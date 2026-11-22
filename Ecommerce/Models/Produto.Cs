namespace Ecommerce.Models;

public class Produto
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public decimal Preco { get; set; }

    public int Estoque { get; set; }

    // Lado "N" do 1:N: cada produto pertence a UMA categoria
    // CategoriaId é a chave estrangeira (a coluna no banco)
    // Categoria é a "navegação" (o objeto, para você usar no código)
    public int CategoriaId { get; set; }
    public Categoria? Categoria { get; set; }
}