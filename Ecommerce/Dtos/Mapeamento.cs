using Ecommerce.Models;

namespace Ecommerce.Dtos;

public static class Mapeamento
{
    public static CategoriaResponse ParaResponse(this Categoria c) => new()
    {
        Id = c.Id,
        Nome = c.Nome,
        Descricao = c.Descricao
    };

    public static ProdutoResponse ParaResponse(this Produto p) => new()
    {
        Id = p.Id,
        Nome = p.Nome,
        Descricao = p.Descricao,
        Preco = p.Preco,
        Estoque = p.Estoque,
        CategoriaId = p.CategoriaId,
        CategoriaNome = p.Categoria?.Nome ?? string.Empty
    };
}