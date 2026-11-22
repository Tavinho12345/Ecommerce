using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data;

public class AppDbContext : DbContext
{
    // As opções (qual banco, onde está) vêm do Program.cs
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    // Cada DbSet vira uma tabela
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Produto> Produtos => Set<Produto>();

    // Aqui ficam os relacionamentos e as restrições do banco
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(e =>
        {
            e.Property(c => c.Nome).IsRequired().HasMaxLength(100);
            e.HasIndex(c => c.Nome).IsUnique(); // nome de categoria não repete
        });

        modelBuilder.Entity<Produto>(e =>
        {
            e.Property(p => p.Nome).IsRequired().HasMaxLength(150);
            e.Property(p => p.Preco).HasPrecision(18, 2);

            // 1:N  Categoria (1)  Produto (N)
            // Restrict: o banco nao apaga uma categoria que ainda tem produtos
            e.HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            // O mesmo nome só pode se repetir em categorias diferentes
            e.HasIndex(p => new { p.CategoriaId, p.Nome }).IsUnique();
        });
    }
}