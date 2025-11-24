using System.Linq;
using System.Collections.Generic;

namespace Fase04.Domain.Entities
{
    public class SeletorQualidade : ISeletorDeProduto
    {
        public Produto Selecionar(List<Produto> produtos)
        {
            return produtos.OrderByDescending(p => p.Qualidade).First();
        }
    }
}
