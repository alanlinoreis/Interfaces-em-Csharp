using Domain.Entities.Models;
using Domain.Entities.Repository;
using System.Text.Json;

namespace Domain.Entities.Service;

public static class ProdutoService
{
    // CRUD completo → exige escrita
    public static Produto? Criar(IWriteRepository<Produto, int> repo, IReadRepository<Produto, int> repoaux, Produto produto)
    {

        if (repoaux.GetById(produto.Id) is not null)
        {
            return null;
        }
        else
             return repo.Add(produto);
        
    } 

    public static IReadOnlyList<Produto> Listar(IReadRepository<Produto, int> repo)
        => repo.ListAll();

    public static Produto? Buscar(IReadRepository<Produto, int> repo, int id)
        => repo.GetById(id);

    public static bool Atualizar(IWriteRepository<Produto, int> repo, Produto produto)
        => repo.Update(produto);

    public static bool Remover(IWriteRepository<Produto, int> repo, int id)
        => repo.Remove(id);

    // ======================
    // 🟦 Métodos extras da Fase 11
    // ======================

    // Buscar por nome
    public static Produto? BuscarPorNome(IReadRepository<Produto, int> repo, string nome)
    {
        return repo.ListAll()
            .FirstOrDefault(p =>
                p.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
    }

    // Buscar por filtro
    public static IReadOnlyList<Produto> BuscarPorFiltro(
        IReadRepository<Produto, int> repo,
        Func<Produto, bool> filtro)
    {
        return repo.ListAll().Where(filtro).ToList();
    }

    // Exportar JSON
    public static void Exportar(IReadRepository<Produto, int> repo, string path)
    {
        var produtos = repo.ListAll();
        var json = JsonSerializer.Serialize(
            produtos,
            new JsonSerializerOptions { WriteIndented = true }
        );
        File.WriteAllText(path, json);
    }

    // IMPORTAR (CORRIGIDO)
    public static void Importar(
        IReadRepository<Produto, int> leitor,
        IWriteRepository<Produto, int> escritor,
        string path)
    {
        if (!File.Exists(path))
            return;

        var json = File.ReadAllText(path);
        if (string.IsNullOrWhiteSpace(json))
            return;

        var produtos = JsonSerializer.Deserialize<List<Produto>>(json);
        if (produtos is null)
            return;

        // limpa o repositório atual
        foreach (var p in leitor.ListAll())
            escritor.Remove(p.Id);

        // insere os importados
        foreach (var p in produtos)
            escritor.Add(p);
    }

    // Stream assíncrono
    public static async IAsyncEnumerable<Produto> StreamAsync(
        IReadRepository<Produto, int> repo,
        [System.Runtime.CompilerServices.EnumeratorCancellation]
        CancellationToken token)
    {
        foreach (var p in repo.ListAll())
        {
            token.ThrowIfCancellationRequested();
            await Task.Delay(400, token);
            yield return p;
        }
    }
}
