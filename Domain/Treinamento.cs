namespace TeamHeartFiap.Domain

{
    // Trocar a classe TreinamentoVm (que estava no folder Domain por engano)
    // para a entidade de domínio Treinamento correta.
    public class Treinamento
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public bool Obrigatorio { get; set; } = false;

        // Coleção de conclusões para o relacionamento com ConclusaoTreinamento
        public ICollection<ConclusaoTreinamento> ConclusoesTreinamento { get; set; } = new List<ConclusaoTreinamento>();
    }

}