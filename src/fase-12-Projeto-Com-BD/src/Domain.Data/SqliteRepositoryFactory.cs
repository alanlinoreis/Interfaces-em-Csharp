using Domain.Entities.Models;
using Domain.Entities.Repository;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data;

public static class SqliteRepositoryFactory
{
    public static (IReadRepository<Produto, int> leitor,
                   IWriteRepository<Produto, int> escritor)
        Create(string? connectionString = null)
    {
        connectionString ??= "Data Source=catalogo.db";

        var options = new DbContextOptionsBuilder<CatalogoDbContext>()
            .UseSqlite(connectionString)
            .Options;

        var context = new CatalogoDbContext(options);

        // Garante que o banco e as tabelas existem
        context.Database.EnsureCreated();

        var repo = new SqliteProdutoRepository(context);

        // Leitura e escrita usam a mesma instância
        return (repo, repo);
    }
}
