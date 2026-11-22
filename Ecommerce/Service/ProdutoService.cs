using Ecommerce.Dtos;
using Ecommerce.Models;

namespace Ecommerce.Services;

public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _produtos;
    private readonly ICategoriaRepository _categorias;

    public ProdutoService(IProdutoRepository produtos, ICategoriaRepository categorias)
    {
        _produtos = produtos;
        _categorias = categorias;
    }

    public async Task<List<ProdutoResponse>> ListarAsync(int? categoriaId)
    {
        var produtos = await _produtos.ListarAsync(categoriaId);
        return produtos.Select(p => p.ParaResponse()).ToList();
    }

    // Devolve null quando o produto não existe
    public async Task<ProdutoResponse?> ObterAsync(int id)
    {
        var produto = await _produtos.ObterAsync(id);
        return produto?.ParaResponse();
    }

    public async Task<ProdutoResponse> CriarAsync(ProdutoRequest request)
    {
        var categoria = await ValidarRegrasAsync(request, idIgnorado: null);

        var produto = new Produto
        {
            Nome = request.Nome.Trim(),
            Descricao = request.Descricao,
            Preco = request.Preco,
            Estoque = request.Estoque,
            CategoriaId = categoria.Id,
            Categoria = categoria
        };

        _produtos.Adicionar(produto);
        await _produtos.SalvarAsync();

        return produto.ParaResponse();
    }

    // Devolve false quando o produto não existe
    public async Task<bool> AtualizarAsync(int id, ProdutoRequest request)
    {
        var produto = await _produtos.ObterAsync(id);

        if (produto is null)
        {
            return false;
        }

        var categoria = await ValidarRegrasAsync(request, idIgnorado: id);

        produto.Nome = request.Nome.Trim();
        produto.Descricao = request.Descricao;
        produto.Preco = request.Preco;
        produto.Estoque = request.Estoque;
        produto.CategoriaId = categoria.Id;
        produto.Categoria = categoria;

        await _produtos.SalvarAsync();

        return true;
    }

    // Devolve false quando o produto não existe
    public async Task<bool> RemoverAsync(int id)
    {
        var produto = await _produtos.ObterAsync(id);

        if (produto is null)
        {
            return false;
        }

        _produtos.Remover(produto);
        await _produtos.SalvarAsync();

        return true;
    }

    // Devolve a categoria já carregada, para o chamador reaproveitar.
    // idIgnorado: no PUT, o próprio produto não conta como duplicado.
    private async Task<Categoria> ValidarRegrasAsync(ProdutoRequest request, int? idIgnorado)
    {
        // Regra 1 (1:N): a categoria precisa existir
        var categoria = await _categorias.ObterAsync(request.CategoriaId);

        if (categoria is null)
        {
            throw new RegraDeNegocioException("A categoria informada não existe.");
        }

        // Regra 2: não repetir o nome dentro da mesma categoria
        if (await _produtos.ExisteAsync(request.Nome.Trim(), request.CategoriaId, idIgnorado))
        {
            throw new RegraDeNegocioException("Já existe um produto com esse nome nessa categoria.");
        }

        return categoria;
    }
}