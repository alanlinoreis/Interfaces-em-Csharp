using Domain.Entities.Models;
using Domain.Entities.Repository;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data;

public class SqliteProdutoRepository :
    IReadRepository<Produto, int>,
    IWriteRepository<Produto, int>,
    IDisposable
{
    private readonly CatalogoDbContext _context;

    public SqliteProdutoRepository(CatalogoDbContext context)
    {
        _context = context;
    }

    // ------------------ LEITURA ------------------
    public Produto? GetById(int id)
    {
        return _context.Produtos
            .AsNoTracking()
            .FirstOrDefault(p => p.Id == id);
    }

    public IReadOnlyList<Produto> ListAll()
    {
        return _context.Produtos
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .ToList();
    }

    // ------------------ ESCRITA ------------------
    public Produto Add(Produto entity)
    {
        _context.Produtos.Add(entity);
        _context.SaveChanges();
        return entity;
    }

    public bool Update(Produto entity)
    {
        var existente = _context.Produtos.Find(entity.Id);
        if (existente is null)
            return false;

        // Copia os valores do record para a entidade rastreada
        _context.Entry(existente).CurrentValues.SetValues(entity);
        _context.SaveChanges();
        return true;
    }

    public bool Remove(int id)
    {
        var existente = _context.Produtos.Find(id);
        if (existente is null)
            return false;

        _context.Produtos.Remove(existente);
        _context.SaveChanges();
        return true;
    }

    // ------------------ Infra ------------------
    public void Dispose()
    {
        _context.Dispose();
    }
}
