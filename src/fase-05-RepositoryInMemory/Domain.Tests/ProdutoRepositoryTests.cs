using Xunit;
using Domain.Entities;
using Domain.Entities.Repositories;
using System.Linq;

namespace Domain.Tests
{
    public class ProdutoRepositoryTests
    {
        private static InMemoryProdutoRepository CreateRepository()
            => new InMemoryProdutoRepository();

        [Fact]
        public void Add_DeveInserirProduto_NoRepositorio()
        {
            var repo = CreateRepository();

            var produto = new Produto(1, "Monitor", 800, 70);
            repo.Add(produto);

            var todos = repo.ListAll();

            Assert.Single(todos);
            Assert.Equal("Monitor", todos[0].Nome);
        }

        [Fact]
        public void GetById_DeveRetornarProduto_QuandoExistir()
        {
            var repo = CreateRepository();

            var produto = new Produto(1, "Teclado", 120, 60);
            repo.Add(produto);

            var encontrado = repo.GetById(1);

            Assert.NotNull(encontrado);
            Assert.Equal("Teclado", encontrado!.Nome);
        }

        [Fact]
        public void GetById_DeveRetornarNull_QuandoNaoExistir()
        {
            var repo = CreateRepository();

            var encontrado = repo.GetById(55);

            Assert.Null(encontrado);
        }

        [Fact]
        public void ListAll_DeveRetornarTodosProdutos()
        {
            var repo = CreateRepository();

            repo.Add(new Produto(1, "Mouse", 80, 50));
            repo.Add(new Produto(2, "Cadeira", 600, 90));

            var lista = repo.ListAll();

            Assert.Equal(2, lista.Count);
            Assert.Contains(lista, x => x.Id == 1);
            Assert.Contains(lista, x => x.Id == 2);
        }

        [Fact]
        public void Update_DeveAtualizarProduto_QuandoExistir()
        {
            var repo = CreateRepository();

            repo.Add(new Produto(1, "Mouse", 80, 50));

            var atualizado = new Produto(1, "Mouse Gamer RGB", 150, 80);

            var resultado = repo.Update(atualizado);

            Assert.True(resultado);
            Assert.Equal("Mouse Gamer RGB", repo.GetById(1)!.Nome);
        }

        [Fact]
        public void Update_DeveRetornarFalso_QuandoProdutoNaoExistir()
        {
            var repo = CreateRepository();

            var atualizado = new Produto(99, "Item Fantasma", 0, 0);

            var resultado = repo.Update(atualizado);

            Assert.False(resultado);
        }

        [Fact]
        public void Remove_DeveRemoverProduto_QuandoExistir()
        {
            var repo = CreateRepository();

            repo.Add(new Produto(1, "SSD", 300, 90));

            var resultado = repo.Remove(1);

            Assert.True(resultado);
            Assert.Empty(repo.ListAll());
        }

        [Fact]
        public void Remove_DeveRetornarFalso_QuandoNaoExistir()
        {
            var repo = CreateRepository();

            var resultado = repo.Remove(500);

            Assert.False(resultado);
        }
    }
}
