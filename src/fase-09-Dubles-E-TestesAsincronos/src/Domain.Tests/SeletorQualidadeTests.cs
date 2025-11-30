using Xunit;
using System.Collections.Generic;
using Domain.Entities.Models;
using Domain.Entities.Seletores;

namespace Domain.Tests
{
    public class SeletorQualidadeTests
    {
        [Fact]
        public void Deve_retornar_o_produto_de_maior_qualidade()
        {
            var produtos = new List<Produto>
            {
                new Produto(1, "A", 200, 50),
                new Produto(2, "B", 180, 80),
                new Produto(3, "C", 150, 60)
            };

            var seletor = new SeletorQualidade();

            var resultado = seletor.Selecionar(produtos);

            Assert.Equal("B", resultado.Nome);
        }
    }
}
