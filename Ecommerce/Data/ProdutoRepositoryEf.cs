using Ecommerce.Models;
using Ecommerce.Services;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data;

public class ProdutoRepositoryEf : IProdutoRepository
{
    private readonly AppDbContext _context;

    public ProdutoRepositoryEf(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Produto>> ListarAsync(int? categoriaId)
    {
        IQueryable<Produto> consulta = _context.Produtos.Include(p => p.Categoria);

        if (categoriaId is not null)
        {
            consulta = consulta.Where(p => p.CategoriaId == categoriaId);
        }

        return await consulta.OrderBy(p => p.Nome).ToListAsync();
    }

    public async Task<Produto?> ObterAsync(int id)
    {
        return await _context.Produtos
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> ExisteAsync(string nome, int categoriaId, int? idIgnorado)
    {
        return await _context.Produtos.AnyAsync(p =>
            p.Nome == nome &&
            p.CategoriaId == categoriaId &&
            (idIgnorado == null || p.Id != idIgnorado));
    }

    public void Adicionar(Produto produto)
    {
        _context.Produtos.Add(produto);
    }

    public void Remover(Produto produto)
    {
        _context.Produtos.Remove(produto);
    }

    public async Task SalvarAsync()
    {
        await _context.SaveChangesAsync();
    }
}