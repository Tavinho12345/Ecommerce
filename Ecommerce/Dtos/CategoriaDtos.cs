using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Dtos;

/// <summary>O que o cliente envia para criar/atualizar uma categoria.</summary>
public class CategoriaRequest
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;
}

/// <summary>O que a API devolve sobre uma categoria.</summary>
public class CategoriaResponse
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
}