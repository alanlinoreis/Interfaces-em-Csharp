namespace Fase04.Domain.Entities
{
    public class Produto
    {
        public string Nome { get; }
        public decimal Preco { get; }
        public int Qualidade { get; }

        public Produto(string nome, decimal preco, int qualidade)
        {
            Nome = nome;
            Preco = preco;
            Qualidade = qualidade;
        }
    }
}
