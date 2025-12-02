using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeamHeartFiap.Services;


namespace TeamHeartFiap.Controllers;

[ApiController]
[Route("api/relatorios")]
public class RelatoriosController : ControllerBase
{
    private readonly IServicoRelatorio _servico;
    public RelatoriosController(IServicoRelatorio servico) => _servico = servico;

    [Authorize(Roles = "Admin")]
    [HttpGet("diversidade")]
    public async Task<IActionResult> ObterDiversidade([FromQuery] DateTime? de, [FromQuery] DateTime? ate)
    {
        var inicio = de ?? DateTime.UtcNow.AddMonths(-1);
        var fim = ate ?? DateTime.UtcNow;
        var resultado = await _servico.GerarRelatorioAsync(inicio, fim);
        return Ok(resultado);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("exportar")]
    public async Task<IActionResult> Exportar([FromQuery] DateTime? de, [FromQuery] DateTime? ate)
    {
        var inicio = de ?? DateTime.UtcNow.AddMonths(-1);
        var fim = ate ?? DateTime.UtcNow;
        var resultado = await _servico.GerarRelatorioAsync(inicio, fim);

        // Exportar CSV simples (em memória)
        var csv = new System.Text.StringBuilder();
        csv.AppendLine("Categoria,Contagem,DataRegistro");
        foreach (var r in resultado)
        {
            csv.AppendLine($"{r.Categoria},{r.Contagem},{r.DataRegistro:O}");
        }

        var bytes = System.Text.Encoding.UTF8.GetBytes(csv.ToString());
        return File(bytes, "text/csv", $"relatorio_diversidade_{DateTime.UtcNow:yyyyMMddHHmmss}.csv");
    }
}