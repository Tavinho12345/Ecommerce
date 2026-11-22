using Ecommerce.Dtos;
using Ecommerce.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _servico;

    public CategoriasController(ICategoriaService servico)
    {
        _servico = servico;
    }

    // GET: api/Categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaResponse>>> GetCategorias()
    {
        return Ok(await _servico.ListarAsync());
    }

    // GET: api/Categorias/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CategoriaResponse>> GetCategoria(int id)
    {
        var categoria = await _servico.ObterAsync(id);

        if (categoria is null)
        {
            return NotFound(); // null -> 404
        }

        return Ok(categoria);
    }

    // GET: api/Categorias/5/produtos   (1:N: os produtos da categoria)
    [HttpGet("{id}/produtos")]
    public async Task<ActionResult<IEnumerable<ProdutoResponse>>> GetProdutosDaCategoria(int id)
    {
        var produtos = await _servico.ListarProdutosAsync(id);

        if (produtos is null)
        {
            return NotFound();
        }

        return Ok(produtos);
    }

    // POST: api/Categorias
    [HttpPost]
    public async Task<ActionResult<CategoriaResponse>> PostCategoria(CategoriaRequest request)
    {
        try
        {
            var criada = await _servico.CriarAsync(request);
            return CreatedAtAction(nameof(GetCategoria), new { id = criada.Id }, criada);
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(ex.Message); // regra quebrada -> 400
        }
    }

    // PUT: api/Categorias/5
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategoria(int id, CategoriaRequest request)
    {
        try
        {
            var atualizada = await _servico.AtualizarAsync(id, request);

            if (!atualizada)
            {
                return NotFound(); // false -> 404
            }

            return NoContent();
        }
        catch (RegraDeNegocioException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    // DELETE: api/Categorias/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoria(int id)
    {
        try
        {
            var removida = await _servico.RemoverAsync(id);

            if (!removida)
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
}