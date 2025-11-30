using Xunit;
using Domain.Entities.Repository;
using Domain.Entities.Models;
using Domain.Entities.Service;

public class CsvProdutoRepositoryTests
{
    private string CriarArquivoTemporario()
    {
        return Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".csv");
    }

    // -------------------------------------------------------
    // 1. Arquivo inexistente → ListAll deve retornar vazio
    // -------------------------------------------------------
    [Fact]
    public void ListAll_quando_arquivo_inexistente_retorna_vazio()
    {
        var path = CriarArquivoTemporario();
        var repo = new CsvProdutoRepository(path);

        var produtos = repo.ListAll();

        Assert.Empty(produtos);
    }

    // -------------------------------------------------------
    // 2. Arquivo vazio → ListAll deve retornar vazio
    // -------------------------------------------------------
    [Fact]
    public void ListAll_quando_arquivo_vazio_retorna_vazio()
    {
        var path = CriarArquivoTemporario();
        File.WriteAllText(path, ""); 

        var repo = new CsvProdutoRepository(path);

        var produtos = repo.ListAll();

        Assert.Empty(produtos);
    }

    // -------------------------------------------------------
    // 3. Add deve criar o registro no CSV
    // -------------------------------------------------------
    [Fact]
    public void Add_deve_inserir_produto_no_csv()
    {
        var path = CriarArquivoTemporario();
        var repo = new CsvProdutoRepository(path);

        var produto = new Produto(1, "TV", 2500m, 90);
        repo.Add(produto);

        var todos = repo.ListAll();

        Assert.Single(todos);
        Assert.Equal("TV", todos[0].Nome);
    }

    // -------------------------------------------------------
    // 4. GetById funcionando
    // -------------------------------------------------------
    [Fact]
    public void GetById_deve_retornar_produto_correto()
    {
        var path = CriarArquivoTemporario();
        var repo = new CsvProdutoRepository(path);

        repo.Add(new Produto(1, "TV", 2000, 70));
        repo.Add(new Produto(2, "Notebook", 5000, 90));

        var p = repo.GetById(2);

        Assert.NotNull(p);
        Assert.Equal("Notebook", p!.Nome);
    }

    // -------------------------------------------------------
    // 5. GetById retorna null quando não houver ID
    // -------------------------------------------------------
    [Fact]
    public void GetById_quando_inexistente_retorna_null()
    {
        var path = CriarArquivoTemporario();
        var repo = new CsvProdutoRepository(path);

        repo.Add(new Produto(1, "TV", 2000, 70));

        var p = repo.GetById(99);

        Assert.Null(p);
    }

    // -------------------------------------------------------
    // 6. Update modifica o registro existente
    // -------------------------------------------------------
    [Fact]
    public void Update_deve_substituir_produto_existente()
    {
        var path = CriarArquivoTemporario();
        var repo = new CsvProdutoRepository(path);

        repo.Add(new Produto(1, "TV", 2000m, 70));

        var ok = repo.Update(new Produto(1, "TV 4K", 3500m, 95));

        var p = repo.GetById(1);

        Assert.True(ok);
        Assert.Equal("TV 4K", p!.Nome);
        Assert.Equal(3500m, p.Preco);
        Assert.Equal(95, p.Qualidade);
    }

    // -------------------------------------------------------
    // 7. Update retorna false se o ID não existe
    // -------------------------------------------------------
    [Fact]
    public void Update_quando_inexistente_retorna_false()
    {
        var path = CriarArquivoTemporario();
        var repo = new CsvProdutoRepository(path);

        repo.Add(new Produto(1, "TV", 2000m, 70));

        var ok = repo.Update(new Produto(2, "X", 123, 1));

        Assert.False(ok);
    }

    // -------------------------------------------------------
    // 8. Remove deve excluir produto
    // -------------------------------------------------------
    [Fact]
    public void Remove_deve_apagar_produto()
    {
        var path = CriarArquivoTemporario();
        var repo = new CsvProdutoRepository(path);

        repo.Add(new Produto(1, "A", 10, 1));
        repo.Add(new Produto(2, "B", 20, 2));

        var removed = repo.Remove(1);

        var todos = repo.ListAll();

        Assert.True(removed);
        Assert.Single(todos);
        Assert.Equal(2, todos[0].Id);
    }

    // -------------------------------------------------------
    // 9. Remove inexistente → false
    // -------------------------------------------------------
    [Fact]
    public void Remove_quando_inexistente_retorna_false()
    {
        var path = CriarArquivoTemporario();
        var repo = new CsvProdutoRepository(path);

        repo.Add(new Produto(1, "A", 10, 1));

        var removed = repo.Remove(99);

        Assert.False(removed);
    }

    // -------------------------------------------------------
    // 10. CSV com vírgulas deve funcionar
    // -------------------------------------------------------
    [Fact]
    public void Add_e_ListAll_devem_suportar_virgulas_no_nome()
    {
        var path = CriarArquivoTemporario();
        var repo = new CsvProdutoRepository(path);

        var nome = "Geladeira, Frost Free";
        repo.Add(new Produto(1, nome, 3000m, 80));

        var p = repo.GetById(1)!;

        Assert.Equal(nome, p.Nome);
    }

    // -------------------------------------------------------
    // 11. CSV com aspas deve funcionar
    // -------------------------------------------------------
    [Fact]
    public void Add_e_ListAll_devem_suportar_aspas_no_nome()
    {
        var path = CriarArquivoTemporario();
        var repo = new CsvProdutoRepository(path);

        var nome = "Fogão \"Professional\" 5 Bocas";
        repo.Add(new Produto(1, nome, 2300m, 70));

        var p = repo.GetById(1)!;

        Assert.Equal(nome, p.Nome);
    }

    // -------------------------------------------------------
    // 12. Integração com ProdutoService (seleção)
    // -------------------------------------------------------
    [Fact]
    public void ProdutoService_ExecutarSelecao_deve_funcionar_com_csv()
    {
        var path = CriarArquivoTemporario();
        var repo = new CsvProdutoRepository(path);

        repo.Add(new Produto(1, "A", 100, 50));
        repo.Add(new Produto(2, "B", 90, 40));
        repo.Add(new Produto(3, "C", 150, 90));

        var escolhido = ProdutoService.ExecutarSelecao(repo, "ECONOMICO");

        Assert.Equal(2, escolhido.Id); // B é o mais barato
    }
}
