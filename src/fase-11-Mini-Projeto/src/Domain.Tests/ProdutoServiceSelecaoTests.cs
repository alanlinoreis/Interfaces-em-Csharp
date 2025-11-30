using Xunit;
using Domain.Entities.Models;
using Domain.Entities.Repository;
using Domain.Entities.Seletores;
using Domain.Entities.Service;

namespace Domain.Tests
{
    public class ProdutoServiceSelecaoTests
    {
        private InMemoryRepository<Produto, int> CriarRepo()
            => new(p => p.Id);

        [Fact]
        public void Deve_selecionar_economico()
        {
            var repo = CriarRepo();

            repo.Add(new Produto(1, "A", 200, 50));
            repo.Add(new Produto(2, "B", 100, 70));

            var servico = new ProdutoSelecaoService();

            var result = servico.Selecionar(repo, ModoSelecao.Economico);

            Assert.Equal("B", result.Nome);
        }

        [Fact]
        public void Deve_selecionar_qualidade()
        {
            var repo = CriarRepo();

            repo.Add(new Produto(1, "A", 200, 50));
            repo.Add(new Produto(2, "B", 100, 80));

            var servico = new ProdutoSelecaoService();

            var result = servico.Selecionar(repo, ModoSelecao.Qualidade);

            Assert.Equal("B", result.Nome);
        }
    }
}
