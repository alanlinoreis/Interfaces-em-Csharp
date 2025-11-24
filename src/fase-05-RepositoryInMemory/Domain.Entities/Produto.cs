namespace Domain.Entities
{
    public class Produto
    {
        public int Id { get; }
        public string Nome { get; }
        public decimal Preco { get; }
        public int Qualidade { get; }

        public Produto(int id, string nome, decimal preco, int qualidade)
        {
            Id = id;
            Nome = nome;
            Preco = preco;
            Qualidade = qualidade;
        }
    }
}
