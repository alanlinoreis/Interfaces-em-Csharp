using Xunit;
using Domain.Entities.Models;
using Domain.Entities.Repository;

namespace Domain.Tests
{
    public class ProdutoRepositoryTests
    {
        private InMemoryRepository<Produto, int> CriarRepo()
            => new(p => p.Id);

        [Fact]
        public void Add_deve_inserir_produto()
        {
            var repo = CriarRepo();

            var p = new Produto(1, "TV", 2000, 8);
            repo.Add(p);

            var resultado = repo.GetById(1);

            Assert.Equal("TV", resultado!.Nome);
        }

        [Fact]
        public void GetById_deve_retornar_null_quando_inexistente()
        {
            var repo = CriarRepo();

            var resultado = repo.GetById(999);

            Assert.Null(resultado);
        }

        [Fact]
        public void ListAll_deve_retornar_todos_os_produtos()
        {
            var repo = CriarRepo();

            repo.Add(new Produto(1, "A", 10, 5));
            repo.Add(new Produto(2, "B", 20, 8));

            var lista = repo.ListAll();

            Assert.Equal(2, lista.Count);
        }

        [Fact]
        public void Update_deve_atualizar_quando_existe()
        {
            var repo = CriarRepo();

            repo.Add(new Produto(1, "A", 10, 5));

            var atualizado = new Produto(1, "A+", 15, 7);
            var ok = repo.Update(atualizado);

            var resultado = repo.GetById(1);

            Assert.True(ok);
            Assert.Equal("A+", resultado?.Nome);
        }

        [Fact]
        public void Update_deve_retornar_false_quando_nao_existe()
        {
            var repo = CriarRepo();

            var naoExiste = new Produto(10, "X", 10, 2);

            var ok = repo.Update(naoExiste);

            Assert.False(ok);
        }

        [Fact]
        public void Remove_deve_excluir_quando_existe()
        {
            var repo = CriarRepo();

            repo.Add(new Produto(1, "A", 10, 5));

            var ok = repo.Remove(1);

            Assert.True(ok);
            Assert.Null(repo.GetById(1));
        }

        [Fact]
        public void Remove_deve_retornar_false_quando_nao_existe()
        {
            var repo = CriarRepo();

            var ok = repo.Remove(123);

            Assert.False(ok);
        }
    }
}
