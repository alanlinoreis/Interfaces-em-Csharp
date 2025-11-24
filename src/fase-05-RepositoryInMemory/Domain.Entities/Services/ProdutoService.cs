using System.Linq;
using Fase05.Domain.Entities.Repositories;

namespace Fase05.Domain.Entities.Services
{
    public class ProdutoService
    {
        private readonly IProdutoRepository _repo;
        private readonly ISeletorDeProduto _seletor;

        public ProdutoService(IProdutoRepository repo, ISeletorDeProduto seletor)
        {
            _repo = repo;
            _seletor = seletor;
        }

        public Produto Registrar(Produto produto)
            => _repo.Add(produto);

        public Produto SelecionarMelhorProduto()
            => _seletor.Selecionar(_repo.ListAll().ToList());
    }
}