using Microsoft.EntityFrameworkCore;
using TeamHeartFiap.Domain;

namespace TeamHeartFiap.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Mapeamento de tabelas/conjuntos de dados
        public DbSet<Candidato> Candidatos => Set<Candidato>();
        public DbSet<MetricaDiversidade> MetricasDiversidade => Set<MetricaDiversidade>();
        public DbSet<Treinamento> Treinamentos => Set<Treinamento>();
        public DbSet<ConclusaoTreinamento> ConclusoesTreinamento => Set<ConclusaoTreinamento>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Nomes explícitos das tabelas para Oracle
            modelBuilder.Entity<Candidato>().ToTable("CANDIDATOS");
            modelBuilder.Entity<MetricaDiversidade>().ToTable("METRICAS_DIVERSIDADE");
            modelBuilder.Entity<Treinamento>().ToTable("TREINAMENTOS");
            modelBuilder.Entity<ConclusaoTreinamento>().ToTable("CONCLUSAO_TREINAMENTO");

            // Configuração da chave primária de ConclusaoTreinamento
            modelBuilder.Entity<ConclusaoTreinamento>()
                .HasKey(ct => ct.Id);

            // Relações (Candidato 1 <-> N ConclusoesTreinamento)
            modelBuilder.Entity<ConclusaoTreinamento>()
                .HasOne(ct => ct.Candidato)
                .WithMany(c => c.ConclusoesTreinamento)
                .HasForeignKey(ct => ct.CandidatoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relações (Treinamento 1 <-> N ConclusoesTreinamento)
            modelBuilder.Entity<ConclusaoTreinamento>()
                .HasOne(ct => ct.Treinamento)
                .WithMany(t => t.ConclusoesTreinamento)
                .HasForeignKey(ct => ct.TreinamentoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Evitar duplicidade de conclusão do mesmo treinamento pelo mesmo candidato
            modelBuilder.Entity<ConclusaoTreinamento>()
                .HasIndex(ct => new { ct.CandidatoId, ct.TreinamentoId })
                .IsUnique();

            // Índices para otimização de busca
            modelBuilder.Entity<Candidato>().HasIndex(c => c.Email);
            modelBuilder.Entity<MetricaDiversidade>().HasIndex(m => m.Categoria);
        }
    }
}
