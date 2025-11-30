using Domain.Entities.Models;
using Domain.Entities.Repository;
using Domain.Entities.Seletores;


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

    // Seleção → só usa leitura!
    public static Produto ExecutarSelecao(
        IReadRepository<Produto, int> repo,
        string tipoSelecao)
    {
        var produtos = repo.ListAll();
        var seletor = SeletorFactory.Criar(tipoSelecao);
        return seletor.Selecionar(produtos);
    }
}
