using Microsoft.AspNetCore.Mvc;
using TeamHeartFiap.Services;
using TeamHeartFiap.ViewModels;

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
    // Corrigido para retornar IActionResult e remover o 'async' (resolve warning CS1998)
    public IActionResult ObterPorId(int id)
    {
        // ideal chamar repositório/serviço; para manter simples, usar repositório direto pode ser adicionado
        return Ok(); // TODO: implementar busca por id se necessário
    }
}