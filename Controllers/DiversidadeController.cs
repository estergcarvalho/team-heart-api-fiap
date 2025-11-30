using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamHeartFiap.Data;
using TeamHeartFiap.Infra;
using TeamHeartFiap.ViewModels;

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
        var q = _db.MetricasDiversidade
            .OrderByDescending(m => m.DataRegistro)
            .Skip((p.Page - 1) * p.PageSize)
            .Take(p.PageSize)
            .Select(m => new MetricaDiversidadeVm { Categoria = m.Categoria, Contagem = m.Contagem });

        var items = await q.AsNoTracking().ToListAsync();
        var total = await _db.MetricasDiversidade.CountAsync();

        return Ok(new { total, pagina = p.Page, tamanho = p.PageSize, items });
    }
}