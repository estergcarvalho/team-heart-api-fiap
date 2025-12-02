using Microsoft.EntityFrameworkCore;
using TeamHeartFiap.Domain;

namespace TeamHeartFiap.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Candidato> Candidatos => Set<Candidato>();
        public DbSet<MetricaDiversidade> MetricasDiversidade => Set<MetricaDiversidade>();
        public DbSet<Treinamento> Treinamentos => Set<Treinamento>();
        public DbSet<ConclusaoTreinamento> ConclusoesTreinamento => Set<ConclusaoTreinamento>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Candidato>().ToTable("CANDIDATOS");
            modelBuilder.Entity<MetricaDiversidade>().ToTable("METRICAS_DIVERSIDADE");
            modelBuilder.Entity<Treinamento>().ToTable("TREINAMENTOS");
            modelBuilder.Entity<ConclusaoTreinamento>().ToTable("CONCLUSAO_TREINAMENTO");

            modelBuilder.Entity<ConclusaoTreinamento>()
                .HasKey(ct => ct.Id);

            modelBuilder.Entity<ConclusaoTreinamento>()
                .HasOne(ct => ct.Candidato)
                .WithMany(c => c.ConclusoesTreinamento)
                .HasForeignKey(ct => ct.CandidatoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ConclusaoTreinamento>()
                .HasOne(ct => ct.Treinamento)
                .WithMany(t => t.ConclusoesTreinamento)
                .HasForeignKey(ct => ct.TreinamentoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ConclusaoTreinamento>()
                .HasIndex(ct => new { ct.CandidatoId, ct.TreinamentoId })
                .IsUnique();

            modelBuilder.Entity<Candidato>().HasIndex(c => c.Email);
            modelBuilder.Entity<MetricaDiversidade>().HasIndex(m => m.Categoria);
        }
    }
}
