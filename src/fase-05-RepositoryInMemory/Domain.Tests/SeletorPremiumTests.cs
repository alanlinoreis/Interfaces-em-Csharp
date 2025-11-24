using Xunit;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Tests
{
    public class SeletorPremiumTests
    {
        [Fact]
        public void Deve_escolher_melhor_qualidade_quando_dentro_do_limite()
        {
            var produtos = new List<Produto>
            {
                new Produto("Barato", 100, 50),
                new Produto("Qualidade", 250, 90)
            };

            var seletor = new SeletorPremium();

            var resultado = seletor.Selecionar(produtos);

            Assert.Equal("Qualidade", resultado.Nome);
        }

        [Fact]
        public void Deve_voltar_para_economico_quando_acima_do_limite()
        {
            var produtos = new List<Produto>
            {
                new Produto("Barato", 100, 50),
                new Produto("Qualidade", 400, 90) // R$300 acima → deve cair no mais barato
            };

            var seletor = new SeletorPremium();

            var resultado = seletor.Selecionar(produtos);

            Assert.Equal("Barato", resultado.Nome);
        }
    }
}
