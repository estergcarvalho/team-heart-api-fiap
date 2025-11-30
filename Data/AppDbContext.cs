using Microsoft.EntityFrameworkCore;
using TeamHeartFiap.Domain; // CORREÇÃO PRINCIPAL: Necessário para DbContext, DbSet, etc.

namespace TeamHeartFiap.Data
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

            // Nomes explícitos das tabelas (compatível com Oracle)
            modelBuilder.Entity<Candidato>().ToTable("CANDIDATOS");
            modelBuilder.Entity<MetricaDiversidade>().ToTable("METRICAS_DIVERSIDADE");
            modelBuilder.Entity<Treinamento>().ToTable("TREINAMENTOS");
            modelBuilder.Entity<ConclusaoTreinamento>().ToTable("CONCLUSAO_TREINAMENTO");

            // Definir Id como PK (se você prefere chave composta, remova Id da entidade)
            modelBuilder.Entity<ConclusaoTreinamento>()
                .HasKey(ct => ct.Id);

            // Relações
            modelBuilder.Entity<ConclusaoTreinamento>()
                .HasOne(ct => ct.Candidato)
                .WithMany(c => c.ConclusoesTreinamento) // assegure que Candidato tenha ICollection<ConclusaoTreinamento> ConclusoesTreinamento
                .HasForeignKey(ct => ct.CandidatoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ConclusaoTreinamento>()
                .HasOne(ct => ct.Treinamento)
                .WithMany(t => t.ConclusoesTreinamento) // assegure que Treinamento tenha ICollection<ConclusaoTreinamento> ConclusoesTreinamento
                .HasForeignKey(ct => ct.TreinamentoId)
                .OnDelete(DeleteBehavior.Cascade);

            // Evitar duplicidade: índice único em (CandidatoId, TreinamentoId)
            modelBuilder.Entity<ConclusaoTreinamento>()
                .HasIndex(ct => new { ct.CandidatoId, ct.TreinamentoId })
                .IsUnique();

            // Índices básicos
            modelBuilder.Entity<Candidato>().HasIndex(c => c.Email);
            modelBuilder.Entity<MetricaDiversidade>().HasIndex(m => m.Categoria);
        }
    }
}