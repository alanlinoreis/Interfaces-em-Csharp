using Domain.Entities.Models;
using Domain.Entities.Repository;
using Domain.Entities.Service;

namespace Domain.App;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Fase 06 — Repository CSV ===\n");

        string path = "produtos.csv";
        var repo = new CsvProdutoRepository(path);

        GarantirArquivoInicial(repo, path);

        ListarProdutos(repo);
        TestarSeletores(repo);
        TestarCrud(repo);

        Console.WriteLine("\nFim da demonstração.");
    }

    // -----------------------------------------------------
    // Garante que o arquivo CSV inicial existe
    // -----------------------------------------------------
    private static void GarantirArquivoInicial(CsvProdutoRepository repo, string path)
    {
        if (File.Exists(path))
            return;

        Console.WriteLine("Arquivo CSV não encontrado. Criando arquivo inicial...\n");

        repo.Add(new Produto(1, "Notebook Gamer", 6500m, 95));
        repo.Add(new Produto(2, "Geladeira Frost Free", 3200m, 80));
        repo.Add(new Produto(3, "Smartphone Pro Max", 4500m, 90));
        repo.Add(new Produto(4, "Smart TV 4K", 2800m, 85));
    }

    // -----------------------------------------------------
    // Lista os produtos do CSV
    // -----------------------------------------------------
    private static void ListarProdutos(CsvProdutoRepository repo)
    {
        Console.WriteLine("Produtos cadastrados:");

        foreach (var p in repo.ListAll())
        {
            Console.WriteLine($"{p.Id} - {p.Nome} - R${p.Preco} - Q{p.Qualidade}");
        }

        Console.WriteLine();
    }

    // -----------------------------------------------------
    // Testa os seletores via ProdutoService
    // -----------------------------------------------------
    private static void TestarSeletores(CsvProdutoRepository repo)
    {
        var economico = ProdutoService.ExecutarSelecao(repo, "ECONOMICO");
        Console.WriteLine($"Mais econômico: {economico.Nome} (R${economico.Preco})");

        var premium = ProdutoService.ExecutarSelecao(repo, "PREMIUM");
        Console.WriteLine($"Premium: {premium.Nome}");

        var qualidade = ProdutoService.ExecutarSelecao(repo, "QUALIDADE");
        Console.WriteLine($"Maior qualidade: {qualidade.Nome}");

        Console.WriteLine();
    }

    // -----------------------------------------------------
    // Testa CRUD completo no CSV
    // -----------------------------------------------------
    private static void TestarCrud(CsvProdutoRepository repo)
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
