using System.Linq;
using System.Collections.Generic;
using Domain.Entities.Models;

namespace Domain.Entities.Seletores
{
    public class SeletorPremium : ISeletorDeProduto
    {
        public Produto Selecionar(IReadOnlyList<Produto> produtos)
        {
            var maisBarato = produtos.OrderBy(p => p.Preco).First();
            var melhorQualidade = produtos.OrderByDescending(p => p.Qualidade).First();

            if (melhorQualidade.Preco - maisBarato.Preco <= 200)
                return melhorQualidade;

            return maisBarato;
        }
    }
}
