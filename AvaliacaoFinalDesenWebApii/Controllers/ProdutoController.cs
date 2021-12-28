using AvaliacaoFinalDesenWebApii.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AvaliacaoFinalDesenWebApii.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ProdutoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //// GET: api/<ProdutoController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<ProdutoController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<ProdutoController>
        [Authorize]
        [HttpPost]
        public void Post([FromBody] Produto produto)
        {
            string connectionString = _configuration.GetSection("Settings").GetSection("Parameters").GetValue<string>("ConnectionString");
            try
            {
                using (var db = new DbConnection(connectionString))
                {
                    string insertCmd = "INSERT INTO PRODUTO (NomeProduto, Categoria, QuantidadeEstoque, PrecoVenda) " +
                        $"VALUES ('{produto.NomeProduto}', '{produto.Categoria}', {produto.QuantidadeEstoque}, {produto.PrecoVenda});";
                    db.Insert(insertCmd);
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }

        //// PUT api/<ProdutoController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<ProdutoController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
