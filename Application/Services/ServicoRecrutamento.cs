using System;
using TeamHeartFiap.Domain;
using TeamHeartFiap.Repositories;
using TeamHeartFiap.ViewModels;

namespace TeamHeartFiap.Services
{
    public class ServicoRecrutamento : IRecrutamentoServico
    {
        private readonly IRepository<Candidato> _repo;

        public ServicoRecrutamento(IRepository<Candidato> repo)
        {
            _repo = repo;
        }

        public async Task<Candidato> EnviarCandidatoAsync(CandidatoCriarVm vm)
        {
            var candidato = new Candidato
            {
                Nome = vm.Nome,
                Email = vm.Email,
                Demografico = vm.Demografico,
                DataCandidatura = DateTime.UtcNow
            };

            if (!string.IsNullOrWhiteSpace(candidato.Demografico) &&
                candidato.Demografico.ToLower().Contains("negro"))
            {
                candidato.PrioridadeDiversidade = true;
            }

            await _repo.AdicionarAsync(candidato);
            await _repo.SalvarAsync();

            return candidato;
        }
    }
}