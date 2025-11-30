using Domain.Entities.Models;
using Domain.Entities.Repository;
using Domain.Entities.Seletores;

namespace Domain.Entities.Service;

public static class ProdutoService
{
    // -----------------------------
    // CRUD
    // -----------------------------
    
    public static Produto Criar(
        IRepository<Produto, int> repo,
        Produto produto)
    {
        return repo.Add(produto);
    }

    public static IReadOnlyList<Produto> Listar(
        IRepository<Produto, int> repo)
    {
        return repo.ListAll();
    }

    public static Produto? Buscar(
        IRepository<Produto, int> repo, int id)
    {
        return repo.GetById(id);
    }

    public static bool Atualizar(
        IRepository<Produto, int> repo, Produto produto)
    {
        return repo.Update(produto);
    }

    public static bool Remover(
        IRepository<Produto, int> repo, int id)
    {
        return repo.Remove(id);
    }

    // ------------------------------------
    // Integração com seletores (Fase 04)
    // ------------------------------------

    public static Produto ExecutarSelecao(
        IRepository<Produto, int> repo,
        string tipoSelecao)
    {
        // Carrega produtos do repositório
        var produtosReadOnly = repo.ListAll();

        // Converte para List<Produto> (compatível com seletores da Fase 04)
        var produtos = produtosReadOnly.ToList();

        // Cria o seletor via factory da Fase 04
        var seletor = SeletorFactory.Criar(tipoSelecao);

        // Executa a seleção
        return seletor.Selecionar(produtos);
    }
}
