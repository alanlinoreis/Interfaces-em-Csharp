using Domain.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Domain.Data;

public class CatalogoDbContext : DbContext
{
    public CatalogoDbContext(DbContextOptions<CatalogoDbContext> options)
        : base(options)
    {
    }

    public DbSet<Produto> Produtos => Set<Produto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var produto = modelBuilder.Entity<Produto>();

        produto.HasKey(p => p.Id);

        produto.Property(p => p.Nome)
               .IsRequired()
               .HasMaxLength(200);

        produto.Property(p => p.Preco)
               .HasPrecision(18, 2);

        produto.Property(p => p.Qualidade)
               .IsRequired();
    }
}
