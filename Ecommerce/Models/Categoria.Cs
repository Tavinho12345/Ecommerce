namespace Ecommerce.Models;

public class Categoria
{
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    // Lado "1" do 1:N: uma categoria tem vários produtos
    public List<Produto> Produtos { get; set; } = [];
}