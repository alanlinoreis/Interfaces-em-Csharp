using System.Linq;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class SeletorPremium : ISeletorDeProduto
    {
        public Produto Selecionar(List<Produto> produtos)
        {
            var maisBarato = produtos.OrderBy(p => p.Preco).First();
            var melhorQualidade = produtos.OrderByDescending(p => p.Qualidade).First();

            if (melhorQualidade.Preco - maisBarato.Preco <= 200)
                return melhorQualidade;

            return maisBarato;
        }
    }
}
