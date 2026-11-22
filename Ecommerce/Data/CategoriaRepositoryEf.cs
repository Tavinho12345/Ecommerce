using Ecommerce.Models;
using Ecommerce.Services;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data;

/// <summary>
/// Implementação do contrato usando Entity Framework Core.
/// </summary>
public class CategoriaRepositoryEf : ICategoriaRepository
{
    private readonly AppDbContext _context;

    public CategoriaRepositoryEf(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Categoria>> ListarAsync()
    {
        return await _context.Categorias.OrderBy(c => c.Nome).ToListAsync();
    }

    public async Task<Categoria?> ObterAsync(int id)
    {
        return await _context.Categorias.FindAsync(id);
    }

    public async Task<Categoria?> ObterComProdutosAsync(int id)
    {
        // Include: traz os produtos junto, na mesma consulta (JOIN)
        return await _context.Categorias
            .Include(c => c.Produtos)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> ExisteAsync(string nome, int? idIgnorado)
    {
        return await _context.Categorias.AnyAsync(c =>
            c.Nome == nome &&
            (idIgnorado == null || c.Id != idIgnorado));
    }

    public async Task<bool> PossuiProdutosAsync(int id)
    {
        return await _context.Produtos.AnyAsync(p => p.CategoriaId == id);
    }

    public void Adicionar(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
    }

    public void Remover(Categoria categoria)
    {
        _context.Categorias.Remove(categoria);
    }

    public async Task SalvarAsync()
    {
        await _context.SaveChangesAsync();
    }
}