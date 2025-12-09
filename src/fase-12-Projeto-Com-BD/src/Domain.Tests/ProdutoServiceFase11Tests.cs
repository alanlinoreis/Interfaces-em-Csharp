using Domain.Entities.Models;
using Domain.Entities.Repository;
using Domain.Entities.Service;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;
using System.IO;

public class ProdutoServiceFase11Tests
{
    // ============================================================
    // UTIL
    // ============================================================
    private InMemoryRepository<Produto, int> CriarRepo()
        => new InMemoryRepository<Produto, int>(p => p.Id);

    private Produto P(int id, string nome, decimal preco, int qualidade)
        => new Produto(id, nome, preco, qualidade);

    // ============================================================
    // BUSCAR POR NOME
    // ============================================================
    [Fact]
    public void BuscarPorNome_deve_retornar_produto_existente()
    {
        var repo = CriarRepo();
        repo.Add(P(1, "Geladeira", 1200m, 80));
        repo.Add(P(2, "Smartphone", 2000m, 90));

        var result = ProdutoService.BuscarPorNome(repo, "gel");

        Assert.NotNull(result);
        Assert.Equal(1, result!.Id);
    }

    [Fact]
    public void BuscarPorNome_retorna_null_quando_nao_existe()
    {
        var repo = CriarRepo();
        repo.Add(P(1, "Mesa", 300m, 50));

        var result = ProdutoService.BuscarPorNome(repo, "Notebook");

        Assert.Null(result);
    }

    // ============================================================
    // BUSCAR POR FILTRO
    // ============================================================
    [Fact]
    public void BuscarPorFiltro_filtra_por_qualidade()
    {
        var repo = CriarRepo();
        repo.Add(P(1, "A", 10, 50));
        repo.Add(P(2, "B", 20, 90));
        repo.Add(P(3, "C", 30, 95));

        var res = ProdutoService.BuscarPorFiltro(repo, p => p.Qualidade >= 90);

        Assert.Equal(2, res.Count);
        Assert.Contains(res, p => p.Id == 2);
        Assert.Contains(res, p => p.Id == 3);
    }

    // ============================================================
    // EXPORTAR
    // ============================================================
    [Fact]
    public void Exportar_deve_gerar_arquivo_json_valido()
    {
        var repo = CriarRepo();
        repo.Add(P(1, "Produto X", 123m, 80));

        string path = Path.GetTempFileName();

        ProdutoService.Exportar(repo, path);

        var texto = File.ReadAllText(path);

        Assert.Contains("Produto X", texto);
        Assert.Contains("123", texto);
    }

    // ============================================================
    // IMPORTAR
    // ============================================================
    [Fact]
    public void Importar_deve_ler_arquivo_e_substituir_conteudo()
    {
        var repo = CriarRepo();

        // arquivo temporário com JSON
        string path = Path.GetTempFileName();

        var produtos = new List<Produto>() {
            P(10, "Mouse", 80m, 70),
            P(20, "Teclado", 150m, 65)
        };

        File.WriteAllText(
            path,
            System.Text.Json.JsonSerializer.Serialize(produtos)
        );

        // CHAMADA CORRIGIDA: passar leitor + escritor + path
        ProdutoService.Importar(repo, repo, path);

        Assert.Equal(2, repo.ListAll().Count);
        Assert.NotNull(repo.GetById(10));
        Assert.NotNull(repo.GetById(20));
    }

    // ============================================================
    // STREAM ASYNC
    // ============================================================
    [Fact]
    public async Task StreamAsync_deve_emitir_itens_em_ordem()
    {
        var repo = CriarRepo();
        repo.Add(P(1, "A", 10, 50));
        repo.Add(P(2, "B", 20, 60));

        using var cts = new CancellationTokenSource(3000);

        var coletados = new List<Produto>();

        await foreach (var p in ProdutoService.StreamAsync(repo, cts.Token))
            coletados.Add(p);

        Assert.Equal(2, coletados.Count);
        Assert.Equal(1, coletados[0].Id);
        Assert.Equal(2, coletados[1].Id);
    }
}
