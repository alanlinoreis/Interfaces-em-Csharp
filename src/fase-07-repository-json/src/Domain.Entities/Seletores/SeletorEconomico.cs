using System.Linq;
using System.Collections.Generic;
using Domain.Entities.Models;

namespace Domain.Entities.Seletores
{
    public class SeletorEconomico : ISeletorDeProduto
    {
        public Produto Selecionar(IReadOnlyList<Produto> produtos)
        {
            return produtos
                .OrderBy(p => p.Preco)
                .First();
        }
    }
}
