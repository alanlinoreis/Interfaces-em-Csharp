using Xunit;
using Domain.Entities.Models;
using Domain.Entities.Service;
using Domain.Entities.Repository;
using Domain.Entities.Seletores;

namespace Domain.Tests
{
    public class ProdutoServiceSelecaoTests
    {
        private InMemoryRepository<Produto, int> CriarRepo()
            => new(p => p.Id);

        [Fact]
        public void ExecutarSelecao_deve_usar_seletor_economico()
        {
            var repo = CriarRepo();

            repo.Add(new Produto(1, "A", 200, 50));
            repo.Add(new Produto(2, "B", 100, 70));

            var result = ProdutoService.ExecutarSelecao(repo, "economico");

            Assert.Equal("B", result.Nome);
        }

        [Fact]
        public void ExecutarSelecao_deve_usar_seletor_qualidade()
        {
            var repo = CriarRepo();

            repo.Add(new Produto(1, "A", 200, 50));
            repo.Add(new Produto(2, "B", 100, 80));

            var result = ProdutoService.ExecutarSelecao(repo, "qualidade");

            Assert.Equal("B", result.Nome);
        }
    }
}
