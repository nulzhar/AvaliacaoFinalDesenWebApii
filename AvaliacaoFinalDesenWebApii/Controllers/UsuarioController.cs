using AvaliacaoFinalDesenWebApii.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Security.Cryptography;
using System.Text;

namespace AvaliacaoFinalDesenWebApii.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IConfiguration _config;
        public UsuarioController(IConfiguration Configuration)
        {
            _config = Configuration;
        }

        [HttpPost]
        public IActionResult Criar([FromBody] Usuario usuario)
        {
            string connectionString = _config["Settings:Parameters:ConnectionString"];
            try
            {
                using (var db = new DbConnection(connectionString))
                {
                    string insertCmd = "INSERT INTO Usuario (NomeUsuario, Senha) " +
                        $"VALUES ('{usuario.NomeUsuario}', '{EncryptorSenha(usuario.Senha)}');";
                    db.Insert(insertCmd);
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest();
            }
            return Ok();
        }

        public string EncryptorSenha(string senha)
        {
            try
            {
                byte[] KeyArray;
                byte[] ToEncryptArray = Encoding.UTF8.GetBytes(senha);
                var key = _config["Settings:CryptoKey"];

                MD5CryptoServiceProvider hashMd5 = new MD5CryptoServiceProvider();
                KeyArray = hashMd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                hashMd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
                tdes.Key = KeyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;


                ICryptoTransform cryptoTransform = tdes.CreateEncryptor();
                byte[] resultadoArray = cryptoTransform.TransformFinalBlock(ToEncryptArray, 0, ToEncryptArray.Length);
                tdes.Clear();
                return Convert.ToBase64String(resultadoArray, 0, resultadoArray.Length);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
