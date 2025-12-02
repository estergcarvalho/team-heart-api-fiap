using System;

namespace TeamHeartFiap.Domain
{
    public class ConclusaoTreinamento
    {
        public int Id { get; set; } 
        
        public int CandidatoId { get; set; } 
        public int TreinamentoId { get; set; }
        
        public DateTime DataConclusao { get; set; } = DateTime.UtcNow;

        public Candidato Candidato { get; set; } = null!;
        public Treinamento Treinamento { get; set; } = null!;
    }
}