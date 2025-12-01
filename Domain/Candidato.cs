using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TeamHeartFiap.Domain
{
    public class Candidato
    {
        [Key] 
        public int Id { get; set; }
        
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public DateTime DataCandidatura { get; set; } = DateTime.UtcNow;
        public string? Demografico { get; set; }
        public bool PrioridadeDiversidade { get; set; } = false;
        
        public ICollection<ConclusaoTreinamento> ConclusoesTreinamento { get; set; } = new List<ConclusaoTreinamento>();
    }
}