using Ecommerce.Models;

namespace Ecommerce.Services;

public interface IProdutoRepository
{
    // Filtro opcional por categoria
    Task<List<Produto>> ListarAsync(int? categoriaId);

    // Já traz a categoria junto
    Task<Produto?> ObterAsync(int id);

    // "Existe um produto com este nome nesta categoria?"
    Task<bool> ExisteAsync(string nome, int categoriaId, int? idIgnorado);

    void Adicionar(Produto produto);
    void Remover(Produto produto);
    Task SalvarAsync();
}