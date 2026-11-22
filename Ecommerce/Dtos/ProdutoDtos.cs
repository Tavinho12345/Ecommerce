using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Dtos;

public class ProdutoRequest
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(150, ErrorMessage = "O nome deve ter no máximo 150 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    [Range(0.01, 1_000_000, ErrorMessage = "O preço deve ser maior que zero.")]
    public decimal Preco { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "O estoque não pode ser negativo.")]
    public int Estoque { get; set; }

    // 1:N: a categoria à qual o produto pertence
    [Range(1, int.MaxValue, ErrorMessage = "Informe a categoria.")]
    public int CategoriaId { get; set; }
}

public class ProdutoResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
    public int CategoriaId { get; set; }
    public string CategoriaNome { get; set; } = string.Empty;
}