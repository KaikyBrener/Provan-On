using ApiProvaOnline.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiProvaOnline.Infrastructure.Data
{
    public class ProvaOnDbContext : DbContext
    {
        public ProvaOnDbContext(DbContextOptions<ProvaOnDbContext> options) : base(options) { }

        public DbSet<Prova> Provas { get; set; }
        public DbSet<Aluno> Alunos { get; set; }
        public DbSet<Professor> Professores { get; set; }
        public DbSet<Administrativo> Administrativos { get; set; }
        public DbSet<Pergunta> Perguntas { get; set; }
        public DbSet<PerguntaObjetiva> PerguntasObjetivas { get; set; }
        public DbSet<AlunoResposta> AlunoRespostas { get; set; }
        public DbSet<AlunoRespostaObjetiva> AlunoRespostasObjetivas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProvaOnDbContext).Assembly);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.IsDeleted = false;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
