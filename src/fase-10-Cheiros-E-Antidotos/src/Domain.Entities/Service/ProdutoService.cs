using Domain.Entities.Models;
using Domain.Entities.Repository;

namespace Domain.Entities.Service;

public static class ProdutoService
{
    // CRUD completo → exige escrita
    public static Produto Criar(IWriteRepository<Produto, int> repo, Produto produto)
        => repo.Add(produto);

    public static IReadOnlyList<Produto> Listar(IReadRepository<Produto, int> repo)
        => repo.ListAll();

    public static Produto? Buscar(IReadRepository<Produto, int> repo, int id)
        => repo.GetById(id);

    public static bool Atualizar(IWriteRepository<Produto, int> repo, Produto produto)
        => repo.Update(produto);

    public static bool Remover(IWriteRepository<Produto, int> repo, int id)
        => repo.Remove(id);
}
