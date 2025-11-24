using System.Linq;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class SeletorEconomico : ISeletorDeProduto
    {
        public Produto Selecionar(List<Produto> produtos)
        {
            return produtos.OrderBy(p => p.Preco).First();
        }
    }
}
