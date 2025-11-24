using System.Linq;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class SeletorQualidade : ISeletorDeProduto
    {
        public Produto Selecionar(List<Produto> produtos)
        {
            return produtos.OrderByDescending(p => p.Qualidade).First();
        }
    }
}
