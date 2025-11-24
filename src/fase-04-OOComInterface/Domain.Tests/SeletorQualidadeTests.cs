using Xunit;
using System.Collections.Generic;
using Fase04.Domain.Entities;

namespace Fase04.Domain.Tests
{
    public class SeletorQualidadeTests
    {
        [Fact]
        public void Deve_retornar_o_produto_de_maior_qualidade()
        {
            var produtos = new List<Produto>
            {
                new Produto("A", 200, 50),
                new Produto("B", 180, 80),
                new Produto("C", 150, 60)
            };

            var seletor = new SeletorQualidade();

            var resultado = seletor.Selecionar(produtos);

            Assert.Equal("B", resultado.Nome);
        }
    }
}
