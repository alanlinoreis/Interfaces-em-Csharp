namespace Domain.Entities.Repository;

// Mantemos para compatibilidade entre fases anteriores
public interface IRepository<T, TId> :
    IReadRepository<T, TId>,
    IWriteRepository<T, TId>
{
}
