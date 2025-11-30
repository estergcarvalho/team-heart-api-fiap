using TeamHeartFiap.Domain;
using TeamHeartFiap.ViewModels;

namespace TeamHeartFiap.Services
{
    public interface IRecrutamentoServico
    {
        Task<Candidato> EnviarCandidatoAsync(CandidatoCriarVm vm);
    }
}