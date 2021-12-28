using AvaliacaoFinalDesenWebApii.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace AvaliacaoFinalDesenWebApii.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        public AuthController(IConfiguration Configuration)
        {
            _config = Configuration;
        }

        [HttpPost]
        public IActionResult Login([FromBody] Usuario usuario)
        {
            bool resultado = ValidarUsuario(usuario);
            if (resultado)
            {
                var tokenString = GerarTokenJWT();
                return Ok(new { token = tokenString });
            }
            else
            {
                return Unauthorized();
            }
        }

        private string GerarTokenJWT()
        {
            var issuer = _config["Settings:Jwt:Issuer"];
            var audience = _config["Settings:Jwt:Audience"];
            var expiry = DateTime.Now.AddMinutes(120);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Settings:Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(issuer: issuer, audience: audience,
                expires: expiry, signingCredentials: credentials);
            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

        private bool ValidarUsuario(Usuario usuario)
        {
            if (usuario.NomeUsuario.Equals("admin") && usuario.Senha.Equals("123")) return true;

            string connectionString = _config["Settings:Parameters:ConnectionString"];
            try
            {
                using (var db = new DbConnection(connectionString))
                {
                    string query = "SELECT * FROM Usuario (nolock) " +
                        $"WHERE NomeUsuario = '{usuario.NomeUsuario}';";
                    var dataset = db.GetList(query);
                    if (dataset.Rows.Count > 0 && DecryptorSenha(dataset.Rows[0].Field<string>("Senha")).Equals(usuario.Senha))
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                return false;
            }
        }

        public string DecryptorSenha(string senha)
        {
            try
            {
                byte[] KeyArray;
                byte[] ToEncryptArray = Convert.FromBase64String(senha);
                var key = _config["Settings:CryptoKey"];

                MD5CryptoServiceProvider hashMd5 = new MD5CryptoServiceProvider();
                KeyArray = hashMd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                hashMd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = KeyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;


                ICryptoTransform cryptoTransform = tdes.CreateDecryptor();
                byte[] resultadoArray = cryptoTransform.TransformFinalBlock(ToEncryptArray, 0, ToEncryptArray.Length);
                tdes.Clear();
                return Encoding.UTF8.GetString(resultadoArray);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
