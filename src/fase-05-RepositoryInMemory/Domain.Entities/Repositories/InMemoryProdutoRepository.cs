namespace Domain.Entities.Repositories
{
    public class InMemoryProdutoRepository : IProdutoRepository
    {
        private readonly Dictionary<int, Produto> _store = new();

        public Produto Add(Produto produto)
        {
            _store[produto.Id] = produto;
            return produto;
        }

        public Produto? GetById(int id)
            => _store.TryGetValue(id, out var p) ? p : null;

        public IReadOnlyList<Produto> ListAll()
            => _store.Values.ToList();

        public bool Update(Produto produto)
        {
            if (!_store.ContainsKey(produto.Id))
                return false;
            _store[produto.Id] = produto;
            return true;
        }

        public bool Remove(int id)
            => _store.Remove(id);
    }

}
