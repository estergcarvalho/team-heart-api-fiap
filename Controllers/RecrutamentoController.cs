using Microsoft.AspNetCore.Mvc;
using TeamHeartFiap.Services;
using TeamHeartFiap.ViewModels;
using TeamHeartFiap.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TeamHeartFiap.Controllers;

[ApiController]
[Route("api/recrutamento")]
public class RecrutamentoController : ControllerBase
{
    private readonly IRecrutamentoServico _servico;
    public RecrutamentoController(IRecrutamentoServico servico) => _servico = servico;

    [HttpPost("enviar")]
    public async Task<IActionResult> Enviar([FromBody] CandidatoCriarVm vm)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var criado = await _servico.EnviarCandidatoAsync(vm);
        return CreatedAtAction(nameof(ObterPorId), new { id = criado.Id }, criado);
    }

    [HttpGet("candidatos/{id:int}")]
    public async Task<IActionResult> ObterPorId(int id, [FromServices] AppDbContext db)
    {
        var c = await db.Candidatos
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (c == null) return NotFound("Candidato não encontrado.");

        return Ok(c);
    }
    
    [HttpGet("candidatos")]
    public async Task<IActionResult> Listar([FromServices] AppDbContext db)
    {
        var lista = await db.Candidatos
            .AsNoTracking()
            .Select(c => new {
                c.Id,
                c.Nome,
                c.Email,
                c.Demografico
            })
            .ToListAsync();

        return Ok(lista);
    }

}