using Domain.Entities.Models;
using Domain.Entities.Repository;
using Domain.Entities.Service;

namespace Domain.App;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Fase 08 — ISP ===\n");

        string path = "produtos.json";
        var repo = new JsonProdutoRepository(path);

        IReadRepository<Produto, int> leitor = repo;
        IWriteRepository<Produto, int> escritor = repo;

        // Criar arquivo inicial se não existir
        GarantirArquivoInicial(repo, path);

        // Listar
        ListarProdutos(repo);

        // Testar seletores
        TestarSeletores(repo);

        // Testar CRUD (com o repo concreto)
        TestarCrud(repo);

        // Testar seleção via serviço
        var melhor = ProdutoService.ExecutarSelecao(leitor, "QUALIDADE");
        Console.WriteLine($"\nMelhor produto (via serviço): {melhor.Nome}");
    }

    // -------------------------------------------------------------
    // Gera o arquivo JSON inicial se não existir
    // -------------------------------------------------------------
    private static void GarantirArquivoInicial(JsonProdutoRepository repo, string path)
    {
        if (File.Exists(path))
            return;

        Console.WriteLine("Arquivo JSON não encontrado. Criando arquivo inicial...\n");

        repo.Add(new Produto(1, "Notebook Gamer", 6500m, 95));
        repo.Add(new Produto(2, "Geladeira Frost Free", 3200m, 80));
        repo.Add(new Produto(3, "Smartphone Pro Max", 4500m, 90));
        repo.Add(new Produto(4, "Smart TV 4K", 2800m, 85));
    }

    // -------------------------------------------------------------
    // Lista todos os produtos
    // -------------------------------------------------------------
    private static void ListarProdutos(JsonProdutoRepository repo)
    {
        Console.WriteLine("Produtos cadastrados:");

        foreach (var p in repo.ListAll())
        {
            Console.WriteLine($"{p.Id} - {p.Nome} - R${p.Preco} - Q{p.Qualidade}");
        }

        Console.WriteLine();
    }

    // -------------------------------------------------------------
    // Testa seletores
    // -------------------------------------------------------------
    private static void TestarSeletores(JsonProdutoRepository repo)
    {
        var economico = ProdutoService.ExecutarSelecao(repo, "ECONOMICO");
        Console.WriteLine($"Mais econômico: {economico.Nome} (R${economico.Preco})");

        var premium = ProdutoService.ExecutarSelecao(repo, "PREMIUM");
        Console.WriteLine($"Premium: {premium.Nome}");

        var qualidade = ProdutoService.ExecutarSelecao(repo, "QUALIDADE");
        Console.WriteLine($"Maior qualidade: {qualidade.Nome}");

        Console.WriteLine();
    }

    // -------------------------------------------------------------
    // Testa CRUD completo
    // -------------------------------------------------------------
    private static void TestarCrud(JsonProdutoRepository repo)
    {
        Console.WriteLine("Testando CRUD:");

        // CREATE
        repo.Add(new Produto(5, "Ventilador Turbo", 250m, 60));
        Console.WriteLine("- Produto 5 adicionado.");

        // READ
        var p5 = repo.GetById(5);
        Console.WriteLine($"- Buscar 5: {p5?.Nome}");

        // UPDATE
        repo.Update(new Produto(5, "Ventilador Turbo 2.0", 299m, 70));
        Console.WriteLine("- Produto 5 atualizado.");

        // DELETE
        repo.Remove(5);
        Console.WriteLine("- Produto 5 removido.");
    }
}
