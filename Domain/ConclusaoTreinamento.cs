using System;

namespace TeamHeartFiap.Domain
{
    public class ConclusaoTreinamento
    {
        // Chave Primária Simples
        public int Id { get; set; } 
        
        // Chaves Estrangeiras (FKs)
        public int CandidatoId { get; set; } // Renomeado de UsuarioId (string) para CandidatoId (int)
        public int TreinamentoId { get; set; }
        
        public DateTime DataConclusao { get; set; } = DateTime.UtcNow;

        // Propriedades de Navegação (Necessárias para o AppDbContext)
        public Candidato Candidato { get; set; } = null!;
        public Treinamento Treinamento { get; set; } = null!;
    }
}