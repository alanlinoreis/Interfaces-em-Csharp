using System.Text.Json;
using Domain.Entities.Models;

namespace Domain.Entities.Repository;

public sealed class JsonProdutoRepository : IRepository<Produto, int>
{
    private readonly string _path;
    private readonly JsonSerializerOptions _options;

    public JsonProdutoRepository(string path)
    {
        _path = path;
        _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }

    // -------------------------------------------------------------
    // CRUD
    // -------------------------------------------------------------

    public Produto Add(Produto entity)
    {
        var produtos = ReadAllInternal();

        // política de ID permanece igual à fase 6: vem do domínio
        produtos.RemoveAll(p => p.Id == entity.Id);
        produtos.Add(entity);

        WriteAllInternal(produtos);
        return entity;
    }

    public Produto? GetById(int id)
    {
        return ReadAllInternal()
            .FirstOrDefault(p => p.Id == id);
    }

    public IReadOnlyList<Produto> ListAll()
    {
        return ReadAllInternal();
    }

    public bool Update(Produto entity)
    {
        var produtos = ReadAllInternal();
        var index = produtos.FindIndex(p => p.Id == entity.Id);

        if (index == -1)
            return false;

        produtos[index] = entity;
        WriteAllInternal(produtos);
        return true;
    }

    public bool Remove(int id)
    {
        var produtos = ReadAllInternal();
        var removed = produtos.RemoveAll(p => p.Id == id) > 0;

        if (removed)
            WriteAllInternal(produtos);

        return removed;
    }

    // -------------------------------------------------------------
    // MANIPULAÇÃO DE ARQUIVO
    // -------------------------------------------------------------

    private List<Produto> ReadAllInternal()
    {
        if (!File.Exists(_path))
            return new List<Produto>();

        var text = File.ReadAllText(_path).Trim();

        if (string.IsNullOrWhiteSpace(text))
            return new List<Produto>();

        try
        {
            var list = JsonSerializer.Deserialize<List<Produto>>(text, _options);
            return list ?? new List<Produto>();
        }
        catch
        {
            // arquivo corrompido → trata como vazio
            return new List<Produto>();
        }
    }

    private void WriteAllInternal(IEnumerable<Produto> produtos)
    {
        var json = JsonSerializer.Serialize(produtos, _options);
        File.WriteAllText(_path, json);
    }
}
