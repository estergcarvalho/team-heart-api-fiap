using System;

namespace TeamHeartFiap.Domain
{
    public class MetricaDiversidade
    {
        public int Id { get; set; }
        public string? Categoria { get; set; }
        public int Contagem { get; set; }
        public DateTime DataRegistro { get; set; } = DateTime.UtcNow;
    }
}