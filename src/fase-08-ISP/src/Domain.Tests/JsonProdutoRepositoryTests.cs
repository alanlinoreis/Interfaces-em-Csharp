using Xunit;
using Domain.Entities.Models;
using Domain.Entities.Repository;
using Domain.Entities.Service;

namespace Domain.Tests;

public class JsonProdutoRepositoryTests
{
    // cria um arquivo temporário exclusivo por teste
    private static string ArquivoTemp()
    {
        return Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString() + ".json");
    }

    // garante limpeza do arquivo ao final
    private static void Limpar(string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }

    // ----------------------------------------------
    // 1. Arquivo inexistente → Lista vazia
    // ----------------------------------------------
    [Fact]
    public void ListAll_Deve_retornar_vazio_quando_arquivo_nao_existe()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        var lista = repo.ListAll();

        Assert.Empty(lista);
        Limpar(path);
    }

    // ----------------------------------------------
    // 2. Arquivo vazio → Lista vazia
    // ----------------------------------------------
    [Fact]
    public void ListAll_Deve_retornar_vazio_quando_arquivo_esta_vazio()
    {
        var path = ArquivoTemp();
        File.WriteAllText(path, "");

        var repo = new JsonProdutoRepository(path);

        var lista = repo.ListAll();

        Assert.Empty(lista);
        Limpar(path);
    }

    // ----------------------------------------------
    // 3. Add → persiste corretamente
    // ----------------------------------------------
    [Fact]
    public void Add_Deve_persistir_produto_no_json()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        repo.Add(new Produto(1, "Notebook", 5000m, 90));

        var lista = repo.ListAll();

        Assert.Single(lista);
        Assert.Equal("Notebook", lista[0].Nome);

        Limpar(path);
    }

    // ----------------------------------------------
    // 4. GetById → retorna item correto
    // ----------------------------------------------
    [Fact]
    public void GetById_Deve_retornar_produto_correto()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        repo.Add(new Produto(1, "TV", 3000m, 80));

        var item = repo.GetById(1);

        Assert.NotNull(item);
        Assert.Equal("TV", item!.Nome);

        Limpar(path);
    }

    // ----------------------------------------------
    // 5. Update → modifica item
    // ----------------------------------------------
    [Fact]
    public void Update_Deve_atualizar_produto()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        repo.Add(new Produto(1, "Fogão", 900m, 70));

        var atualizado = new Produto(1, "Fogão Inox", 1200m, 75);

        var resultado = repo.Update(atualizado);

        Assert.True(resultado);

        var item = repo.GetById(1);
        Assert.Equal("Fogão Inox", item!.Nome);
        Assert.Equal(1200m, item.Preco);

        Limpar(path);
    }

    // ----------------------------------------------
    // 6. Update → retorna false se não existe
    // ----------------------------------------------
    [Fact]
    public void Update_Deve_retornar_false_quando_produto_nao_existe()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        var resultado = repo.Update(new Produto(99, "Nada", 0, 0));

        Assert.False(resultado);
        Limpar(path);
    }

    // ----------------------------------------------
    // 7. Remove → remove item
    // ----------------------------------------------
    [Fact]
    public void Remove_Deve_excluir_produto()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        repo.Add(new Produto(1, "Cadeira", 150m, 40));

        var removido = repo.Remove(1);

        Assert.True(removido);
        Assert.Empty(repo.ListAll());

        Limpar(path);
    }

    // ----------------------------------------------
    // 8. Remove → retorna false quando ID não existe
    // ----------------------------------------------
    [Fact]
    public void Remove_Deve_retornar_false_quando_id_inexistente()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        var removido = repo.Remove(123);

        Assert.False(removido);
        Limpar(path);
    }

    // ----------------------------------------------
    // 9. Textos complexos → vírgulas e aspas
    // ----------------------------------------------
    [Fact]
    public void Deve_salvar_e_ler_textos_com_virgulas_e_aspas()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        var p = new Produto(
            1,
            "Monitor 27\", UltraWide, \"Gamer\"",
            2500m,
            95
        );

        repo.Add(p);

        var lido = repo.GetById(1);

        Assert.NotNull(lido);
        Assert.Equal("Monitor 27\", UltraWide, \"Gamer\"", lido!.Nome);

        Limpar(path);
    }

    // ----------------------------------------------
    // 10. Integração com ProdutoService
    // ----------------------------------------------
    [Fact]
    public void ProdutoService_Deve_funcionar_com_repository_json()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        repo.Add(new Produto(1, "Barato", 100m, 40));
        repo.Add(new Produto(2, "Melhor Qualidade", 350m, 95));

        var selecionado = ProdutoService.ExecutarSelecao(repo, "QUALIDADE");

        Assert.Equal("Melhor Qualidade", selecionado.Nome);

        Limpar(path);
    }
}
