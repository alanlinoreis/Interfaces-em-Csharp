namespace Domain.Entities.Repositories
{
    public interface IProdutoRepository
    {
        Produto Add(Produto produto);
        Produto? GetById(int id);
        IReadOnlyList<Produto> ListAll();
        bool Update(Produto produto);
        bool Remove(int id);
    }
}
