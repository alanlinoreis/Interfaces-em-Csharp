using Domain.Entities.Models;
using Domain.Entities.Repository;
using Domain.Entities.Service;
using Domain.Entities.Seletores;

namespace Domain.App;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== Fase 10 — Cheiros e Antídotos ===\n");

        string path = "produtos.json";
        var repo = new JsonProdutoRepository(path);

        // Agora dividimos responsabilidades:
        IReadRepository<Produto, int> leitor = repo;
        IWriteRepository<Produto, int> escritor = repo;

        var selecaoService = new ProdutoSelecaoService();

        // Criar arquivo inicial se não existir
        GarantirArquivoInicial(repo, path);

        // Listar produtos
        ListarProdutos(leitor);

        // Testar seletores
        TestarSeletores(leitor, selecaoService);

        // Testar CRUD completo
        TestarCrud(escritor, leitor);

        // Seleção “final”
        var melhor = selecaoService.Selecionar(leitor, ModoSelecao.Qualidade);
        Console.WriteLine($"\nMelhor produto (via serviço): {melhor.Nome}");
    }

    // -------------------------------------------------------------
    private static void GarantirArquivoInicial(IWriteRepository<Produto, int> escritor, string path)
    {
        if (File.Exists(path))
        {
            var json = File.ReadAllText(path);
            if (!string.IsNullOrWhiteSpace(json) && json.Trim() != "[]")
                return; // já tem dados válidos
        }

        // arquivo vazio → popula


        Console.WriteLine("Arquivo JSON não encontrado. Criando arquivo inicial...\n");

        escritor.Add(new Produto(1, "Notebook Gamer", 6500m, 95));
        escritor.Add(new Produto(2, "Geladeira Frost Free", 3200m, 80));
        escritor.Add(new Produto(3, "Smartphone Pro Max", 4500m, 90));
        escritor.Add(new Produto(4, "Smart TV 4K", 2800m, 85));
    }

    // -------------------------------------------------------------
    private static void ListarProdutos(IReadRepository<Produto, int> leitor)
    {
        Console.WriteLine("Produtos cadastrados:");

        foreach (var p in leitor.ListAll())
            Console.WriteLine($"{p.Id} - {p.Nome} - R${p.Preco} - Q{p.Qualidade}");

        Console.WriteLine();
    }

    // -------------------------------------------------------------
    private static void TestarSeletores(
        IReadRepository<Produto, int> leitor,
        ProdutoSelecaoService selecaoService)
    {
        var economico = selecaoService.Selecionar(leitor, ModoSelecao.Economico);
        Console.WriteLine($"Mais econômico: {economico.Nome} (R${economico.Preco})");

        var premium = selecaoService.Selecionar(leitor, ModoSelecao.Premium);
        Console.WriteLine($"Premium: {premium.Nome}");

        var qualidade = selecaoService.Selecionar(leitor, ModoSelecao.Qualidade);
        Console.WriteLine($"Maior qualidade: {qualidade.Nome}");

        Console.WriteLine();
    }

    // -------------------------------------------------------------
    private static void TestarCrud(
        IWriteRepository<Produto, int> escritor,
        IReadRepository<Produto, int> leitor)
    {
        Console.WriteLine("Testando CRUD:");

        // CREATE
        escritor.Add(new Produto(5, "Ventilador Turbo", 250m, 60));
        Console.WriteLine("- Produto 5 adicionado.");

        // READ
        var p5 = leitor.GetById(5);
        Console.WriteLine($"- Buscar 5: {p5?.Nome}");

        // UPDATE
        escritor.Update(new Produto(5, "Ventilador Turbo 2.0", 299m, 70));
        Console.WriteLine("- Produto 5 atualizado.");

        // DELETE
        escritor.Remove(5);
        Console.WriteLine("- Produto 5 removido.");
    }
}
