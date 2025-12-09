using System.Text.Json;
using Domain.Entities.Models;
using Domain.Entities.Repository;

namespace Domain.Entities.Repository
{
    public class JsonProdutoRepository :
        IReadRepository<Produto, int>,
        IWriteRepository<Produto, int>
    {
        private readonly string _path;
        private readonly JsonSerializerOptions _opt =
            new() { WriteIndented = true };

        public JsonProdutoRepository(string path)
        {
            _path = path;

            // 🔥 ANTÍDOTO: garantir criação automática do arquivo (compatível com testes)
            if (!File.Exists(_path))
            {
                SalvarArquivo(new List<Produto>());   // cria arquivo vazio
            }
        }

        // -------------------- Persistência --------------------

        private List<Produto> CarregarArquivo()
        {
            if (!File.Exists(_path))
                return new List<Produto>();

            var json = File.ReadAllText(_path);
            return JsonSerializer.Deserialize<List<Produto>>(json, _opt)
                   ?? new List<Produto>();
        }

        private void SalvarArquivo(List<Produto> produtos)
        {
            var json = JsonSerializer.Serialize(produtos, _opt);
            File.WriteAllText(_path, json);
        }

        // -------------------- CRUD --------------------

        public Produto Add(Produto produto)
        {
            var list = CarregarArquivo();

            list.Add(produto);

            SalvarArquivo(list);

            return produto;
        }

        public Produto? GetById(int id)
        {
            return CarregarArquivo().FirstOrDefault(p => p.Id == id);
        }

        public IReadOnlyList<Produto> ListAll()
        {
            return CarregarArquivo();
        }

        public bool Remove(int id)
        {
            var list = CarregarArquivo();
            var item = list.FirstOrDefault(p => p.Id == id);
            if (item == null) return false;

            list.Remove(item);
            SalvarArquivo(list);
            return true;
        }

        public bool Update(Produto produto)
        {
            var list = CarregarArquivo();
            var index = list.FindIndex(p => p.Id == produto.Id);
            if (index == -1) return false;

            list[index] = produto;
            SalvarArquivo(list);
            return true;
        }
    }
}
