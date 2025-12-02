using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TeamHeartFiap.Infrastructure.Data;
using TeamHeartFiap.Domain;             
using TeamHeartFiap.ViewModels;         
using System.Security.Claims;
using TeamHeartFiap.ViewModels;

namespace TeamHeartFiap.Controllers
{
    [ApiController]
    [Route("api/treinamentos")]
    public class TreinamentosController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TreinamentosController(AppDbContext db)
        {
            _db = db;
        }

        // GET api/treinamentos?page=1&pageSize=10
        [HttpGet]
        public async Task<IActionResult> Obter(
            [FromQuery] PaginacaoParams p,
            CancellationToken cancellationToken = default)
        {
            var query = _db.Treinamentos
                .AsNoTracking()
                .OrderBy(t => t.Titulo)
                .Skip((p.Page - 1) * p.PageSize)
                .Take(p.PageSize)
                .Select(t => new TreinamentoVm
                {
                    Id = t.Id,
                    Titulo = t.Titulo,
                    Obrigatorio = t.Obrigatorio
                });

            var itens = await query.ToListAsync(cancellationToken);

            var total = await _db.Treinamentos
                .AsNoTracking()
                .CountAsync(cancellationToken);

            return Ok(new
            {
                total,
                pagina = p.Page,
                tamanho = p.PageSize,
                itens
            });
        }

        // POST api/treinamentos/5/concluir?candidateId=123
        [Authorize]
        [HttpPost("{id:int}/concluir")]
        public async Task<IActionResult> Concluir(
            int id,
            [FromQuery] int? candidateId, 
            CancellationToken cancellationToken = default)
        {
            var treinamento = await _db.Treinamentos
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (treinamento == null)
                return NotFound(new { mensagem = "Treinamento não encontrado." });

            int candidatoIdResolved = 0;
            var sub = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                      ?? User?.FindFirst("sub")?.Value;

            if (!string.IsNullOrWhiteSpace(sub) && int.TryParse(sub, out var parsed))
            {
                candidatoIdResolved = parsed;
            }
            else if (candidateId.HasValue)
            {
                candidatoIdResolved = candidateId.Value;
            }
            else
            {
                return BadRequest(new
                {
                    mensagem = "Id do candidato não fornecido. Passe candidateId na query ou configure o claim 'sub' com o id numérico."
                });
            }

            var candidatoExistente = await _db.Candidatos
                .AsNoTracking()
                .AnyAsync(c => c.Id == candidatoIdResolved, cancellationToken);

            if (!candidatoExistente)
                return BadRequest(new { mensagem = "Candidato informado não existe." });

            var conclusao = new ConclusaoTreinamento
            {
                TreinamentoId = id,
                CandidatoId = candidatoIdResolved,
                DataConclusao = DateTime.UtcNow
            };

            _db.ConclusoesTreinamento.Add(conclusao);
            await _db.SaveChangesAsync(cancellationToken);

            return NoContent();
        }
        
        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] TreinamentoVm vm)
        {
            var t = new Treinamento
            {
                Titulo = vm.Titulo,
                Obrigatorio = vm.Obrigatorio
            };

            _db.Treinamentos.Add(t);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Obter), new { id = t.Id }, t);
        }
    }
    
}
