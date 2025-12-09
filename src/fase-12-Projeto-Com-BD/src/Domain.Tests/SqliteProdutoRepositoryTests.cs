using System;
using System.IO;
using System.Linq;
using Domain.Data;
using Domain.Entities.Models;
using Domain.Entities.Repository;
using Xunit;

namespace Domain.Tests;

public class SqliteProdutoRepositoryTests
{
    // Cria um caminho único para cada banco de teste
    private string BancoTemp()
        => Path.Combine(Path.GetTempPath(), $"catalogo-tests-{Guid.NewGuid()}.db");

    // Remove arquivos do SQLite (db + wal + shm)
    private void Limpar(string path)
    {
        void Apagar(string p)
        {
            if (File.Exists(p))
                File.Delete(p);
        }

        Apagar(path);
        Apagar(path + "-wal");
        Apagar(path + "-shm");
    }

    // --------------------------------------------------------------
    // 1. Add + ListAll devem funcionar no SQLite
    //    (SqliteRepositoryFactory + CatalogoDbContext + SqliteProdutoRepository)
    // --------------------------------------------------------------
    [Fact]
    public void Add_e_ListAll_devem_funcionar_no_sqlite()
    {
        var path = BancoTemp();
        var connectionString = $"Data Source={path}";

        var (leitor, escritor) = SqliteRepositoryFactory.Create(connectionString);

        escritor.Add(new Produto(1, "Notebook", 5000m, 90));
        escritor.Add(new Produto(2, "Smartphone", 2500m, 85));

        var todos = leitor.ListAll();

        Assert.Equal(2, todos.Count);
        Assert.Contains(todos, p => p.Id == 1 && p.Nome == "Notebook");
        Assert.Contains(todos, p => p.Id == 2 && p.Nome == "Smartphone");

        if (leitor is IDisposable disp)
            disp.Dispose();

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 2. GetById deve retornar o produto correto
    // --------------------------------------------------------------
    [Fact]
    public void GetById_deve_retornar_produto_correspondente()
    {
        var path = BancoTemp();
        var connectionString = $"Data Source={path}";

        var (leitor, escritor) = SqliteRepositoryFactory.Create(connectionString);

        escritor.Add(new Produto(10, "Geladeira", 3200m, 80));

        var encontrado = leitor.GetById(10);

        Assert.NotNull(encontrado);
        Assert.Equal("Geladeira", encontrado!.Nome);
        Assert.Equal(3200m, encontrado.Preco);
        Assert.Equal(80, encontrado.Qualidade);

        if (leitor is IDisposable disp)
            disp.Dispose();

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 3. GetById deve retornar null quando não existe
    // --------------------------------------------------------------
    [Fact]
    public void GetById_deve_retornar_null_quando_nao_existe()
    {
        var path = BancoTemp();
        var connectionString = $"Data Source={path}";

        var (leitor, escritor) = SqliteRepositoryFactory.Create(connectionString);

        var encontrado = leitor.GetById(999);

        Assert.Null(encontrado);

        if (leitor is IDisposable disp)
            disp.Dispose();

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 4. Update deve alterar os dados do produto
    // --------------------------------------------------------------
    [Fact]
    public void Update_deve_atualizar_produto_no_sqlite()
    {
        var path = BancoTemp();
        var connectionString = $"Data Source={path}";

        var (leitor, escritor) = SqliteRepositoryFactory.Create(connectionString);

        escritor.Add(new Produto(1, "Antigo", 100m, 50));

        var atualizado = new Produto(1, "Atualizado", 150m, 70);
        var ok = escritor.Update(atualizado);

        Assert.True(ok);

        var depois = leitor.GetById(1);

        Assert.NotNull(depois);
        Assert.Equal("Atualizado", depois!.Nome);
        Assert.Equal(150m, depois.Preco);
        Assert.Equal(70, depois.Qualidade);

        if (leitor is IDisposable disp)
            disp.Dispose();

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 5. Update deve retornar false quando o produto não existe
    // --------------------------------------------------------------
    [Fact]
    public void Update_deve_retornar_false_quando_produto_nao_existe()
    {
        var path = BancoTemp();
        var connectionString = $"Data Source={path}";

        var (leitor, escritor) = SqliteRepositoryFactory.Create(connectionString);

        var p = new Produto(999, "Inexistente", 10m, 10);

        var ok = escritor.Update(p);

        Assert.False(ok);

        if (leitor is IDisposable disp)
            disp.Dispose();

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 6. Remove deve excluir produto existente
    // --------------------------------------------------------------
    [Fact]
    public void Remove_deve_excluir_produto()
    {
        var path = BancoTemp();
        var connectionString = $"Data Source={path}";

        var (leitor, escritor) = SqliteRepositoryFactory.Create(connectionString);

        escritor.Add(new Produto(1, "Pra Remover", 50m, 40));

        var ok = escritor.Remove(1);

        Assert.True(ok);
        Assert.Null(leitor.GetById(1));
        Assert.Empty(leitor.ListAll());

        if (leitor is IDisposable disp)
            disp.Dispose();

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 7. Remove deve retornar false quando produto não existe
    // --------------------------------------------------------------
    [Fact]
    public void Remove_deve_retornar_false_quando_nao_existe()
    {
        var path = BancoTemp();
        var connectionString = $"Data Source={path}";

        var (leitor, escritor) = SqliteRepositoryFactory.Create(connectionString);

        var ok = escritor.Remove(123);

        Assert.False(ok);

        if (leitor is IDisposable disp)
            disp.Dispose();

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 8. Factory deve criar o arquivo do banco (CatalogoDbContext.EnsureCreated)
    // --------------------------------------------------------------
    [Fact]
    public void SqliteRepositoryFactory_deve_criar_arquivo_de_banco()
    {
        var path = BancoTemp();
        var connectionString = $"Data Source={path}";

        var (leitor, escritor) = SqliteRepositoryFactory.Create(connectionString);

        // Força alguma operação, caso o provider só crie arquivo no primeiro acesso
        _ = leitor.ListAll();

        Assert.True(File.Exists(path));

        if (leitor is IDisposable disp)
            disp.Dispose();

        Limpar(path);
    }
}
