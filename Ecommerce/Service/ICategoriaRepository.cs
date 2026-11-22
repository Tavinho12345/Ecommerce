using Ecommerce.Models;

namespace Ecommerce.Services;

/// <summary>
/// O que o serviço precisa saber sobre as categorias guardadas.
/// Nada aqui fala de banco, SQL ou Entity Framework.
/// </summary>
public interface ICategoriaRepository
{
    Task<List<Categoria>> ListarAsync();
    Task<Categoria?> ObterAsync(int id);

    // Categoria já com a lista de produtos carregada (lado "1" do 1:N)
    Task<Categoria?> ObterComProdutosAsync(int id);

    // "Existe uma categoria com este nome?"
    Task<bool> ExisteAsync(string nome, int? idIgnorado);

    // "Esta categoria tem produtos?" (para impedir a remoção)
    Task<bool> PossuiProdutosAsync(int id);

    void Adicionar(Categoria categoria);
    void Remover(Categoria categoria);

    // Grava de uma vez tudo o que foi adicionado, alterado ou removido
    Task SalvarAsync();
}