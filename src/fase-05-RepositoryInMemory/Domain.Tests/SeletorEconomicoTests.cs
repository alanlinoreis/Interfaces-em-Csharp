using Xunit;
using System.Collections.Generic;
using Domain.Entities;

namespace Domain.Tests
{
    public class SeletorEconomicoTests
    {
        [Fact]
        public void Deve_retornar_o_produto_mais_barato()
        {
            var produtos = new List<Produto>
            {
                new Produto("A", 200, 50),
                new Produto("B", 100, 70),
                new Produto("C", 150, 60)
            };

            var seletor = new SeletorEconomico();

            var resultado = seletor.Selecionar(produtos);

            Assert.Equal("B", resultado.Nome);
        }
    }
}
