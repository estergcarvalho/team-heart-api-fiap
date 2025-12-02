namespace TeamHeartFiap.Domain

{

    public class Treinamento
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public bool Obrigatorio { get; set; } = false;

        public ICollection<ConclusaoTreinamento> ConclusoesTreinamento { get; set; } = new List<ConclusaoTreinamento>();
    }

}