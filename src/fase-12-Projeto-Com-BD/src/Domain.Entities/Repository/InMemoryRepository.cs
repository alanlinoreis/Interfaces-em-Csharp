using Domain.Entities.Models;

namespace Domain.Entities.Repository;

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
        => _store.TryGetValue(id, out var entity) ? entity : default;

    public IReadOnlyList<T> ListAll()
        => _store.Values.ToList();

    public bool Update(T entity)
    {
        var id = _getId(entity);

        if (!_store.ContainsKey(id))
            return false;

        _store[id] = entity;
        return true;
    }

    public bool Remove(TId id)
        => _store.Remove(id);
}
