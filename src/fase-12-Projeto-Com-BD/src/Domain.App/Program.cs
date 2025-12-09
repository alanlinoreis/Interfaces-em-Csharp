using System.Linq;
using Domain.Entities.Models;
using Domain.Entities.Repository;
using Domain.Entities.Service;
using Domain.Entities.Seletores;
using Domain.Data;

namespace Domain.App;

public class Program
{
    private static readonly string jsonPath = "produtos.json";

    public static void Main(string[] args)
    {
        Console.WriteLine("=== Fase 12 — Mini-projeto de Consolidação com Banco de dados ===\n");

        var usarJson = args.Contains("--json", StringComparer.OrdinalIgnoreCase);

        IReadRepository<Produto, int> leitor;
        IWriteRepository<Produto, int> escritor;

        if (usarJson)
        {
            var repo = new JsonProdutoRepository(jsonPath);
            leitor = repo;
            escritor = repo;
        }
        else
        {
            var (leitorDb, escritorDb) = SqliteRepositoryFactory.Create();
            leitor = leitorDb;
            escritor = escritorDb;
        }

        GarantirArquivoInicial(leitor, escritor);

        MenuPrincipal(leitor, escritor);
    }

    // ============================================================
    // MENU INTERATIVO
    // ============================================================
    private static void MenuPrincipal(
        IReadRepository<Produto, int> leitor,
        IWriteRepository<Produto, int> escritor)
    {
        while (true)
        {
            Console.WriteLine("\n=== MENU PRINCIPAL ===");
            Console.WriteLine("1 - Listar produtos");
            Console.WriteLine("2 - Adicionar produto");
            Console.WriteLine("3 - Buscar por nome");
            Console.WriteLine("4 - Atualizar produto");
            Console.WriteLine("5 - Remover produto");
            Console.WriteLine("6 - Selecionar (Enum)");
            Console.WriteLine("7 - Exportar JSON");
            Console.WriteLine("8 - Importar JSON");
            Console.WriteLine("9 - Stream Assíncrono");
            Console.WriteLine("0 - Sair");
            Console.Write("Escolha: ");

            var opcao = Console.ReadLine();
            Console.WriteLine();

            switch (opcao)
            {
                case "1": ListarProdutos(leitor); break;
                case "2": AdicionarProduto(escritor); break;
                case "3": BuscarPorNome(leitor); break;
                case "4": AtualizarProduto(escritor, leitor); break;
                case "5": RemoverProduto(escritor); break;
                case "6": Selecionar(leitor); break;
                case "7": Exportar(leitor); break;
                case "8": Importar(leitor, escritor); break;
                case "9": RodarStreamAsync(leitor).Wait(); break;
                case "0": return;
                default: Console.WriteLine("❌ Opção inválida!"); break;
            }
        }
    }

    // ============================================================
    private static void ListarProdutos(IReadRepository<Produto, int> leitor)
    {
        Console.WriteLine("=== Lista de Produtos ===\n");

        var produtos = ProdutoService.Listar(leitor);

        if (produtos.Count == 0)
        {
            Console.WriteLine("Nenhum produto cadastrado.");
            return;
        }

        foreach (var p in produtos)
        {
            Console.WriteLine($"{p.Id} - {p.Nome} - R${p.Preco} - Q{p.Qualidade}");
        }
    }

    // ============================================================
    private static void AdicionarProduto(IWriteRepository<Produto, int> escritor)
    {
        Console.WriteLine("=== Adicionar Produto ===");

        Console.Write("ID (número): ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("❌ ID inválido!");
            return;
        }

        Console.Write("Nome: ");
        string nome = Console.ReadLine()!;

        Console.Write("Preço (decimal): ");
        if (!decimal.TryParse(Console.ReadLine(), out decimal preco))
        {
            Console.WriteLine("❌ Preço inválido!");
            return;
        }

        Console.Write("Qualidade (0-100): ");
        if (!int.TryParse(Console.ReadLine(), out int qualidade) ||
            qualidade < 0 || qualidade > 100)
        {
            Console.WriteLine("❌ Qualidade inválida! Digite de 0 a 100.");
            return;
        }

        ProdutoService.Criar(escritor, new Produto(id, nome, preco, qualidade));

        Console.WriteLine("✔ Produto adicionado!");
    }

    private static void BuscarPorNome(IReadRepository<Produto, int> leitor)
    {
        Console.Write("Nome para busca: ");
        string nome = Console.ReadLine()!;

        var p = ProdutoService.BuscarPorNome(leitor, nome);

        if (p == null)
            Console.WriteLine("❌ Nenhum produto encontrado.");
        else
            Console.WriteLine($"{p.Id} - {p.Nome} - R${p.Preco} - Q{p.Qualidade}");
    }

    private static void AtualizarProduto(
        IWriteRepository<Produto, int> escritor,
        IReadRepository<Produto, int> leitor)
    {
        Console.Write("ID do produto a atualizar: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("❌ ID inválido!");
            return;
        }

        var existente = ProdutoService.Buscar(leitor,id);
        if (existente == null)
        {
            Console.WriteLine("❌ Produto não encontrado.");
            return;
        }

        Console.WriteLine($"Produto atual: {existente.Id} - {existente.Nome} - R${existente.Preco} - Q{existente.Qualidade}");

        Console.Write("Novo nome (vazio = manter): ");
        string novoNome = Console.ReadLine()!;
        if (string.IsNullOrWhiteSpace(novoNome))
            novoNome = existente.Nome;

        Console.Write("Novo preço (vazio = manter): ");
        string precoStr = Console.ReadLine()!;
        decimal novoPreco;
        if (string.IsNullOrWhiteSpace(precoStr))
            novoPreco = existente.Preco;
        else if (!decimal.TryParse(precoStr, out novoPreco))
        {
            Console.WriteLine("❌ Preço inválido!");
            return;
        }

        Console.Write("Nova qualidade (0-100, vazio = manter): ");
        string qualidadeStr = Console.ReadLine()!;
        int novaQualidade;
        if (string.IsNullOrWhiteSpace(qualidadeStr))
            novaQualidade = existente.Qualidade;
        else if (!int.TryParse(qualidadeStr, out novaQualidade) ||
                 novaQualidade < 0 || novaQualidade > 100)
        {
            Console.WriteLine("❌ Qualidade inválida!");
            return;
        }

        var atualizado = new Produto(existente.Id, novoNome, novoPreco, novaQualidade);

        bool ok = ProdutoService.Atualizar(escritor, atualizado);
        Console.WriteLine(ok ? "✔ Produto atualizado!" : "❌ Falha ao atualizar.");
    }

    private static void RemoverProduto(IWriteRepository<Produto, int> escritor)
    {
        Console.Write("ID (número): ");
        if (!int.TryParse(Console.ReadLine(), out int id))
        {
            Console.WriteLine("❌ ID inválido!");
            return;
        }

        bool ok = ProdutoService.Remover(escritor, id);
        Console.WriteLine(ok ? "✔ Produto removido!" : "❌ Produto não encontrado.");
    }

    // ============================================================
    private static void Selecionar(IReadRepository<Produto, int> leitor)
    {
        Console.WriteLine("1 - Econômico");
        Console.WriteLine("2 - Premium");
        Console.WriteLine("3 - Qualidade");
        Console.Write("Modo: ");

        var op = Console.ReadLine();

        ModoSelecao modo = op switch
        {
            "1" => ModoSelecao.Economico,
            "2" => ModoSelecao.Premium,
            _ => ModoSelecao.Qualidade
        };

        var s = new ProdutoSelecaoService();
        var melhor = s.Selecionar(leitor, modo);

        Console.WriteLine(melhor == null
            ? "Nenhum produto encontrado"
            : $"Selecionado: {melhor.Nome} (R${melhor.Preco})");
    }

    // ============================================================
    private static void Exportar(IReadRepository<Produto, int> leitor)
    {
        Console.Write("Arquivo de destino (ex: export.json): ");
        string path = Console.ReadLine()!;

        ProdutoService.Exportar(leitor, path);

        Console.WriteLine("✔ Exportado!");
    }

    private static void Importar(
        IReadRepository<Produto, int> leitor,
        IWriteRepository<Produto, int> escritor)
    {
        Console.Write("Arquivo de origem (ex: import.json): ");
        string path = Console.ReadLine()!;

        ProdutoService.Importar(leitor, escritor, path);

        Console.WriteLine("✔ Import concluído!");
    }

    // ============================================================
    private static async Task RodarStreamAsync(IReadRepository<Produto, int> leitor)
    {
        Console.WriteLine("🌀 Streaming assíncrono (5s)...");

        using var cts = new CancellationTokenSource(5000);

        try
        {
            await foreach (var p in ProdutoService.StreamAsync(leitor, cts.Token))
                Console.WriteLine($"[stream] {p.Nome}");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Stream cancelado (timeout)");
        }
    }

    // ============================================================
    private static void GarantirArquivoInicial(
        IReadRepository<Produto, int> leitor,
        IWriteRepository<Produto, int> escritor)
    {
        // Se já existir algum produto no repositório (JSON ou SQLite), não faz nada
        if (leitor.ListAll().Any())
            return;

        ProdutoService.Criar(escritor, new Produto(1, "Notebook Gamer", 6500m, 95));
        ProdutoService.Criar(escritor, new Produto(2, "Geladeira Frost Free", 3200m, 80));
        ProdutoService.Criar(escritor, new Produto(3, "Smartphone Pro Max", 4500m, 90));
        ProdutoService.Criar(escritor, new Produto(4, "Smart TV 4K", 2800m, 85));
    }
}
