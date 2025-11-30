using Xunit;
using Domain.Entities.Models;
using Domain.Entities.Service;
using Domain.Entities.Repository;

namespace Domain.Tests
{
    public class ProdutoServiceTests
    {
        private InMemoryRepository<Produto, int> CriarRepo()
            => new(p => p.Id);

        [Fact]
        public void Criar_deve_inserir_produto()
        {
            var repo = CriarRepo();

            ProdutoService.Criar(repo, new Produto(1, "TV", 2000, 8));

            var encontrado = ProdutoService.Buscar(repo, 1);

            Assert.NotNull(encontrado);
            Assert.Equal("TV", encontrado!.Nome);
        }

        [Fact]
        public void Listar_deve_retornar_todos()
        {
            var repo = CriarRepo();

            ProdutoService.Criar(repo, new Produto(1, "A", 10, 5));
            ProdutoService.Criar(repo, new Produto(2, "B", 20, 8));

            var lista = ProdutoService.Listar(repo);

            Assert.Equal(2, lista.Count);
        }

        [Fact]
        public void Remover_deve_excluir_quando_existe()
        {
            var repo = CriarRepo();

            ProdutoService.Criar(repo, new Produto(1, "A", 10, 5));

            var ok = ProdutoService.Remover(repo, 1);

            Assert.True(ok);
            Assert.Null(ProdutoService.Buscar(repo, 1));
        }
    }
}
