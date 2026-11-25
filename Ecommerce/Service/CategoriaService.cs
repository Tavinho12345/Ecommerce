using Ecommerce.Dtos;
using Ecommerce.Models;

namespace Ecommerce.Services;

/// O "cozinheiro" das categorias: conhece as regras do negócio.
/// Não sabe nada de HTTP nem de Entity Framework.
public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _repositorio;

    public CategoriaService(ICategoriaRepository repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<List<CategoriaResponse>> ListarAsync()
    {
        var categorias = await _repositorio.ListarAsync();
        return categorias.Select(c => c.ParaResponse()).ToList();
    }

    // Devolve null quando a categoria não existe
    public async Task<CategoriaResponse?> ObterAsync(int id)
    {
        var categoria = await _repositorio.ObterAsync(id);
        return categoria?.ParaResponse();
    }

    // Devolve null quando a categoria não existe
    public async Task<List<ProdutoResponse>?> ListarProdutosAsync(int id)
    {
        var categoria = await _repositorio.ObterComProdutosAsync(id);

        if (categoria is null)
        {
            return null;
        }

        return categoria.Produtos
            .Select(p =>
            {
                var resposta = p.ParaResponse();
                resposta.CategoriaNome = categoria.Nome;
                return resposta;
            })
            .ToList();
    }

    public async Task<CategoriaResponse> CriarAsync(CategoriaRequest request)
    {
        await ValidarRegrasAsync(request, idIgnorado: null);

        var categoria = new Categoria
        {
            Nome = request.Nome.Trim(),
            Descricao = request.Descricao
        };

        _repositorio.Adicionar(categoria);
        await _repositorio.SalvarAsync();

        return categoria.ParaResponse();
    }

    // Devolve false quando a categoria não existe
    public async Task<bool> AtualizarAsync(int id, CategoriaRequest request)
    {
        var categoria = await _repositorio.ObterAsync(id);

        if (categoria is null)
        {
            return false;
        }

        await ValidarRegrasAsync(request, idIgnorado: id);

        categoria.Nome = request.Nome.Trim();
        categoria.Descricao = request.Descricao;

        await _repositorio.SalvarAsync();

        return true;
    }

    // Devolve false quando a categoria não existe
    public async Task<bool> RemoverAsync(int id)
    {
        var categoria = await _repositorio.ObterAsync(id);

        if (categoria is null)
        {
            return false;
        }

        // Regra 2: não se remove categoria que ainda tem produtos
        if (await _repositorio.PossuiProdutosAsync(id))
        {
            throw new RegraDeNegocioException("Não é possível remover uma categoria que possui produtos.");
        }

        _repositorio.Remover(categoria);
        await _repositorio.SalvarAsync();

        return true;
    }

    // As regras de negócio moram aqui num lugar só
    // idIgnorado: no PUT a própria categoria não conta como duplicada
    private async Task ValidarRegrasAsync(CategoriaRequest request, int? idIgnorado)
    {
        // Regra 1: o nome da categoria é único
        if (await _repositorio.ExisteAsync(request.Nome.Trim(), idIgnorado))
        {
            throw new RegraDeNegocioException("Já existe uma categoria com esse nome.");
        }
    }
}
