namespace Ecommerce.Services;

/// Lançada quando uma regra de negócio é quebrada.
/// A mensagem é escrita para quem usa a API.
public class RegraDeNegocioException : Exception
{
    public RegraDeNegocioException(string mensagem) : base(mensagem)
    {
    }
}
