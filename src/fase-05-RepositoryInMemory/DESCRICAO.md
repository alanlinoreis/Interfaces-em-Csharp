# 🧩 Fase 05 — Repository InMemory (contrato + implementação em coleção)
## Projeto: Seletor de Produtos por Preço e Qualidade

---

> Nesta Fase 5 os **snippets** mostram `ProdutoWithId` (ou `Produto` com `int Id`) **somente como exemplo** para documentar o contrato e os testes. Isso preserva sua escolha de manter a entidade real sem Id, conforme solicitado.

---

## 🎯 Objetivo
Introduzir o padrão **Repository** como ponto único de acesso a dados, com uma implementação **InMemory** (coleção) que permite testar sem I/O. O cliente deve falar apenas com o Repository, nunca com coleções internas.

---

## 📁 Entregáveis desta fase
- Pasta: `src/fase-04-repository-inmemory/` (crie no repositório)
- Arquivo obrigatório: `DESCRICAO.md` (este arquivo)
- Conteúdo do `.md`:
  - Diagrama leve (ASCII)
  - Snippets C#:
    - contrato do Repository
    - implementação InMemory (genérica)
    - serviço/cliente usando o repository
    - testes unitários (xUnit) cobrindo operações e fronteiras
  - Documentação da política de ID
  - Checklist e critérios de aceite

> Lembrete: **não** criar arquivos `.cs` reais nesta fase — somente incluir os snippets no `.md`.

---

## 🧾 Diagrama leve (ASCII)

```
Cliente/Serviço --> IRepository<T, TId> --> InMemoryRepository<T, TId> --> Dictionary<TId, T> (coleção)
```

---

## 🔌 Contrato genérico do Repository (snippet)

```csharp
public interface IRepository<T, TId>
{
    T Add(T entity);
    T? GetById(TId id);
    IReadOnlyList<T> ListAll();
    bool Update(T entity);
    bool Remove(TId id);
}
```

> Observações:
> - Expõe apenas operações de acesso a dados — **sem lógica de negócio**.
> - Retorna `IReadOnlyList<T>` para evitar exposição de coleções mutáveis.

---

## 🧠 Implementação InMemory (snippet)

```csharp
public sealed class InMemoryRepository<T, TId> : IRepository<T, TId>
    where TId : notnull
{
    private readonly Dictionary<TId, T> _store = new();
    private readonly Func<T, TId> _getId;

    public InMemoryRepository(Func<T, TId> getId)
    {
        _getId = getId ?? throw new ArgumentNullException(nameof(getId));
    }

    public T Add(T entity)
    {
        var id = _getId(entity);
        _store[id] = entity;
        return entity;
    }

    public T? GetById(TId id)
    {
        return _store.TryGetValue(id, out var entity) ? entity : default;
    }

    public IReadOnlyList<T> ListAll()
    {
        return _store.Values.ToList();
    }

    public bool Update(T entity)
    {
        var id = _getId(entity);
        if (!_store.ContainsKey(id))
            return false;
        _store[id] = entity;
        return true;
    }

    public bool Remove(TId id)
    {
        return _store.Remove(id);
    }
}
```

---

## 🔎 Política de ID (documente a escolha)
Neste repositório já definido você optou por **manter a entidade real sem `Id`**, portanto:

- **Nesta Fase 5**: os exemplos de Repository usam um tipo de entidade de exemplo com `int Id` (ou `ProdutoWithId`) nos snippets para demonstrar contrato, testes e uso.
- **Na prática**: se você migrar para implementar o Repository real (Fase 7+), decida se:
  - o `Id` vem do domínio (o objeto já possui Id), ou
  - o Repository gera Id automaticamente (incremental/GUID).
- Documente essa decisão no README raiz quando for implementar de fato.

---

## 🧩 Exemplo de domínio (apenas no snippet)
*(Nota: isto é **apenas** exemplo para a documentação — sua `domain.entities.Produto` real continua sem Id.)*

```csharp
public sealed class ProdutoWithId
{
    public int Id { get; }
    public string Nome { get; }
    public decimal Preco { get; }
    public int Qualidade { get; }

    public ProdutoWithId(int id, string nome, decimal preco, int qualidade)
    {
        Id = id;
        Nome = nome;
        Preco = preco;
        Qualidade = qualidade;
    }
}
```

---

## 🏷️ Serviço de domínio que usa só o Repository (snippet)

```csharp
public static class ProdutoService
{
    public static ProdutoWithId Registrar(IRepository<ProdutoWithId, int> repo, ProdutoWithId produto)
    {
        // validações de domínio (ex.: nome obrigatório) podem existir aqui
        return repo.Add(produto);
    }

    public static IReadOnlyList<ProdutoWithId> ListarTodos(IRepository<ProdutoWithId, int> repo)
        => repo.ListAll();
}
```

---

## ▶️ Composição (exemplo Program — snippet)

```csharp
public static class Program
{
    public static void Main()
    {
        // Factory simples do Repository para ProdutoWithId (id extraído via lambda)
        IRepository<ProdutoWithId, int> repo = new InMemoryRepository<ProdutoWithId, int>(p => p.Id);

        ProdutoService.Registrar(repo, new ProdutoWithId(1, "Produto A", 100m, 50));
        ProdutoService.Registrar(repo, new ProdutoWithId(2, "Produto B", 250m, 90));

        var all = ProdutoService.ListarTodos(repo);
        Console.WriteLine("Produtos cadastrados:");
        foreach (var p in all)
            Console.WriteLine($"#{p.Id} - {p.Nome} (Q:{p.Qualidade})");
    }
}
```

---

## 🧪 Testes unitários (xUnit — snippets)

> Observação: os testes abaixo usam `ProdutoWithId` apenas como entidade de exemplo.

```csharp
public class InMemoryRepositoryTests
{
    private static InMemoryRepository<ProdutoWithId, int> CreateRepo()
        => new InMemoryRepository<ProdutoWithId, int>(p => p.Id);

    [Fact]
    public void Add_Then_ListAll_ShouldReturnOneItem()
    {
        var repo = CreateRepo();
        repo.Add(new ProdutoWithId(1, "Livro A", 100m, 50));
        var all = repo.ListAll();
        Assert.Single(all);
        Assert.Equal(1, all[0].Id);
    }

    [Fact]
    public void GetById_Existing_ShouldReturnEntity()
    {
        var repo = CreateRepo();
        repo.Add(new ProdutoWithId(1, "Livro A", 100m, 50));
        var found = repo.GetById(1);
        Assert.NotNull(found);
        Assert.Equal("Livro A", found!.Nome);
    }

    [Fact]
    public void GetById_Missing_ShouldReturnNull()
    {
        var repo = CreateRepo();
        var found = repo.GetById(99);
        Assert.Null(found);
    }

    [Fact]
    public void Update_Existing_ShouldReturnTrue()
    {
        var repo = CreateRepo();
        repo.Add(new ProdutoWithId(1, "Livro A", 100m, 50));
        var updated = repo.Update(new ProdutoWithId(1, "Livro A (Revisto)", 100m, 50));
        Assert.True(updated);
        Assert.Equal("Livro A (Revisto)", repo.GetById(1)!.Nome);
    }

    [Fact]
    public void Update_Missing_ShouldReturnFalse()
    {
        var repo = CreateRepo();
        var updated = repo.Update(new ProdutoWithId(1, "Livro A", 100m, 50));
        Assert.False(updated);
    }

    [Fact]
    public void Remove_Existing_ShouldReturnTrue()
    {
        var repo = CreateRepo();
        repo.Add(new ProdutoWithId(1, "Livro A", 100m, 50));
        var removed = repo.Remove(1);
        Assert.True(removed);
        Assert.Empty(repo.ListAll());
    }

    [Fact]
    public void Remove_Missing_ShouldReturnFalse()
    {
        var repo = CreateRepo();
        var removed = repo.Remove(99);
        Assert.False(removed);
    }
}
```

---

## ✅ Checklist de aceitação (rubrica enxuta)
- [ ] Contrato do Repository claro e coeso
- [ ] Implementação InMemory correta (sem I/O)
- [ ] Cliente/serviço depende apenas do Repository
- [ ] Testes unitários (snippets) cobrem operações e fronteiras
- [ ] README raiz atualizado com instruções de execução/testes

---

## ❗ Pitfalls a evitar
- Não colocar regras de negócio dentro do Repository (mantenha-o focado em acesso a dados).
- Não expor coleções mutáveis (use `IReadOnlyList<T>`).
- Documentar claramente a política de Id (quem gera: domínio ou repository).

---

## 📎 Arquivo de referência (Lousa da Fase 5)
Conteúdo original usado nesta descrição: `/mnt/data/Lousa - Fase 5 - Repository In Memory.pdf`. fileciteturn1file0
