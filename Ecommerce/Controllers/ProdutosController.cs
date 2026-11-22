using Ecommerce.Dtos;
using Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _servico;

    public ProdutosController(IProdutoService servico)
    {
        _servico = servico;
    }

    // GET: api/Produtos
    // GET: api/Produtos?categoriaId=1   (filtro opcional)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProdutoResponse>>> GetProdutos([FromQuery] int? categoriaId)
    {
        return Ok(await _servico.ListarAsync(categoriaId));
    }

    // GET: api/Produtos/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ProdutoResponse>> GetProduto(int id)
    {
        var produto = await _servico.ObterAsync(id);

        if (produto is null)
        {
            return NotFound();
        }

        return Ok(produto);
    }

    // POST: api/Produtos
    [HttpPost]
    public async Task<ActionResult<ProdutoResponse>> PostProduto(ProdutoRequest request)
    {
        try
        {
            var criado = await _servico.CriarAsync(request);
            return CreatedAtAction(nameof(GetProduto), new { id = criado.Id }, criado);
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // PUT: api/Produtos/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduto(int id, ProdutoRequest request)
    {
        try
        {
            var atualizado = await _servico.AtualizarAsync(id, request);

            if (!atualizado)
            {
                return NotFound();
            }

            return NoContent();
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/Produtos/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduto(int id)
    {
        var removido = await _servico.RemoverAsync(id);

        if (!removido)
        {
            return NotFound();
        }

        return NoContent();
    }
}