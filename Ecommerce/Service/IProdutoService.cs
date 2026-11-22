using Ecommerce.Dtos;

namespace Ecommerce.Services;

public interface IProdutoService
{
    Task<List<ProdutoResponse>> ListarAsync(int? categoriaId);
    Task<ProdutoResponse?> ObterAsync(int id);
    Task<ProdutoResponse> CriarAsync(ProdutoRequest request);
    Task<bool> AtualizarAsync(int id, ProdutoRequest request);
    Task<bool> RemoverAsync(int id);
}