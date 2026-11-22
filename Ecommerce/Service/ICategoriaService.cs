using Ecommerce.Dtos;

namespace Ecommerce.Services;

// "cardápio": o que o serviço de categorias sabe fazer
public interface ICategoriaService
{
    Task<List<CategoriaResponse>> ListarAsync();
    Task<CategoriaResponse?> ObterAsync(int id);

    // 1:N: os produtos de uma categoria. null quando a categoria não existe.
    Task<List<ProdutoResponse>?> ListarProdutosAsync(int id);

    Task<CategoriaResponse> CriarAsync(CategoriaRequest request);
    Task<bool> AtualizarAsync(int id, CategoriaRequest request);
    Task<bool> RemoverAsync(int id);
}