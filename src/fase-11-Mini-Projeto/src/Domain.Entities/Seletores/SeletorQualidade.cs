using System.Linq;
using System.Collections.Generic;
using Domain.Entities.Models;

namespace Domain.Entities.Seletores
{
    public class SeletorQualidade : ISeletorDeProduto
    {
        public Produto Selecionar(IReadOnlyList<Produto> produtos)
        {
            return produtos
                .OrderByDescending(p => p.Qualidade)
                .First();
        }
    }
}
