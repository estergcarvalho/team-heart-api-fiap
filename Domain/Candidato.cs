using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeamHeartFiap.Domain
{
    public class Candidato
    {
        [Key] // Garante que o Entity Framework reconheça Id como chave primária
        public int Id { get; set; }
        
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public DateTime DataCandidatura { get; set; } = DateTime.UtcNow;
        public string? Demografico { get; set; }
        public bool PrioridadeDiversidade { get; set; } = false;

        // ** CORREÇÃO CRÍTICA **
        // Esta coleção é necessária para resolver o símbolo 'ConclusoesTreinamento' 
        // no AppDbContext, que mapeia a relação com ConclusaoTreinamento.
        public ICollection<ConclusaoTreinamento> ConclusoesTreinamento { get; set; } = new List<ConclusaoTreinamento>();
    }
}