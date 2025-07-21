using Microsoft.EntityFrameworkCore;
using Secretaria.Domain.Entities;

namespace Secretaria.Infrastructure.Persistence
{
    public class SecretariaDbContext : DbContext
    {
        public SecretariaDbContext(DbContextOptions<SecretariaDbContext> options)
            : base(options)
        {
        }

        public DbSet<Matricula> Matriculas { get; set; }
        public DbSet<Turma> Turmas { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Administrador> Administradores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Aluno>().HasIndex(a => a.CPF).IsUnique();
            modelBuilder.Entity<Aluno>().HasIndex(a => a.Email).IsUnique();

            modelBuilder.Entity<Matricula>().HasIndex(m => new { m.AlunoId, m.TurmaId }).IsUnique();

            modelBuilder.Entity<Administrador>().HasIndex(a => a.Email).IsUnique();
        }
    }
}
