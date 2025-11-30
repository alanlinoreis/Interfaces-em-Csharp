using Xunit;
using System;
using System.IO;
using System.Linq;
using Domain.Entities.Models;
using Domain.Entities.Repository;
using Domain.Entities.Seletores;
using Domain.Entities.Service;

namespace Domain.Tests;

public class JsonProdutoRepositoryTests
{
    // Utilitário: cria arquivo temporário
    private string ArquivoTemp()
    {
        string path = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + ".json");
        return path;
    }

    // Utilitário: apaga arquivo ao final
    private void Limpar(string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }

    // --------------------------------------------------------------
    // 1. Add deve inserir
    // --------------------------------------------------------------
    [Fact]
    public void Add_deve_inserir()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        var prod = new Produto(1, "TV", 2000m, 80);
        repo.Add(prod);

        var encontrado = repo.GetById(1);

        Assert.NotNull(encontrado);
        Assert.Equal("TV", encontrado!.Nome);

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 2. Update deve atualizar quando existe
    // --------------------------------------------------------------
    [Fact]
    public void Update_deve_atualizar()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        repo.Add(new Produto(1, "A", 10m, 5));
        repo.Update(new Produto(1, "A+", 15m, 7));

        var atualizado = repo.GetById(1);

        Assert.NotNull(atualizado);
        Assert.Equal("A+", atualizado!.Nome);
        Assert.Equal(15m, atualizado.Preco);
        Assert.Equal(7, atualizado.Qualidade);

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 3. Update deve retornar false quando não existe
    // --------------------------------------------------------------
    [Fact]
    public void Update_deve_retornar_false_quando_inexistente()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        var ok = repo.Update(new Produto(99, "X", 10m, 1));

        Assert.False(ok);

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 4. Remove deve excluir
    // --------------------------------------------------------------
    [Fact]
    public void Remove_deve_excluir()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        repo.Add(new Produto(1, "A", 10m, 5));

        var ok = repo.Remove(1);

        Assert.True(ok);
        Assert.Null(repo.GetById(1));

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 5. Remove deve retornar false quando id não existe
    // --------------------------------------------------------------
    [Fact]
    public void Remove_deve_retornar_false_quando_inexistente()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        var ok = repo.Remove(999);

        Assert.False(ok);

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 6. ListAll deve listar tudo
    // --------------------------------------------------------------
    [Fact]
    public void ListAll_deve_retornar_todos()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        repo.Add(new Produto(1, "A", 10m, 5));
        repo.Add(new Produto(2, "B", 20m, 7));

        var lista = repo.ListAll();

        Assert.Equal(2, lista.Count);
        Assert.Contains(lista, x => x.Id == 1);
        Assert.Contains(lista, x => x.Id == 2);

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 7. GetById deve retornar null quando não existe
    // --------------------------------------------------------------
    [Fact]
    public void GetById_deve_retornar_null_quando_inexistente()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        var encontrado = repo.GetById(999);

        Assert.Null(encontrado);

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 8. Arquivo deve ser criado automaticamente
    // --------------------------------------------------------------
    [Fact]
    public void Arquivo_deve_ser_criado_automaticamente()
    {
        var path = ArquivoTemp();

        Assert.False(File.Exists(path));

        var repo = new JsonProdutoRepository(path);

        Assert.True(File.Exists(path));

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 9. Dados devem persistir entre leituras
    // --------------------------------------------------------------
    [Fact]
    public void Dados_devem_persistir()
    {
        var path = ArquivoTemp();

        {
            var repo = new JsonProdutoRepository(path);
            repo.Add(new Produto(1, "Persistido", 99m, 90));
        }

        {
            var repo2 = new JsonProdutoRepository(path);
            var prod = repo2.GetById(1);

            Assert.NotNull(prod);
            Assert.Equal("Persistido", prod!.Nome);
        }

        Limpar(path);
    }

    // --------------------------------------------------------------
    // 10. TESTE AJUSTADO — ProdutoSelecaoService + ModoSelecao
    // --------------------------------------------------------------
    [Fact]
    public void ProdutoSelecaoService_Deve_funcionar_com_repository_json()
    {
        var path = ArquivoTemp();
        var repo = new JsonProdutoRepository(path);

        repo.Add(new Produto(1, "Barato", 100m, 40));
        repo.Add(new Produto(2, "Melhor Qualidade", 350m, 95));

        var svc = new ProdutoSelecaoService();
        var selecionado = svc.Selecionar(repo, ModoSelecao.Qualidade);

        Assert.Equal("Melhor Qualidade", selecionado.Nome);

        Limpar(path);
    }
}
