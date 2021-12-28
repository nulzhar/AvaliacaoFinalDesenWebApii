using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AvaliacaoFinalDesenWebApii.Models
{
    public class Produto
    {
        public int IdProduto { get; set; }
        public string NomeProduto { get; set; }
        public string Categoria { get; set; }
        public int QuantidadeEstoque { get; set; }
        public decimal PrecoVenda { get; set; }
    }
}
