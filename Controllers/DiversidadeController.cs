using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamHeartFiap.Infrastructure.Data;
using TeamHeartFiap.Domain;
using TeamHeartFiap.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace TeamHeartFiap.Controllers;

[ApiController]
[Route("api/diversidade")]
public class DiversidadeController : ControllerBase
{
    private readonly AppDbContext _db;
    public DiversidadeController(AppDbContext db) => _db = db;

    [HttpGet("metricas")]
    public async Task<IActionResult> ObterMetricas([FromQuery] PaginacaoParams p)
    {
        var query = _db.MetricasDiversidade
            .OrderByDescending(m => m.DataRegistro)
            .Skip((p.Page - 1) * p.PageSize)
            .Take(p.PageSize)
            .Select(m => new MetricaDiversidadeVm
            {
                Categoria = m.Categoria,
                Contagem = m.Contagem
            });

        var items = await query.AsNoTracking().ToListAsync();
        var total = await _db.MetricasDiversidade.CountAsync();

        return Ok(new
        {
            total,
            pagina = p.Page,
            tamanho = p.PageSize,
            items
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("metricas")]
    public async Task<IActionResult> CriarMetrica([FromBody] MetricaDiversidadeVm vm)
    {
        var metrica = new MetricaDiversidade
        {
            Categoria = vm.Categoria,
            Contagem = vm.Contagem,
            DataRegistro = DateTime.UtcNow
        };

        _db.MetricasDiversidade.Add(metrica);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(ObterMetricas), new { id = metrica.Id }, metrica);
    }
}