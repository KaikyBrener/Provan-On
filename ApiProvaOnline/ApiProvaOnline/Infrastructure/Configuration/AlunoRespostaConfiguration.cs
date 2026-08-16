using ApiProvaOnline.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiProvaOnline.Infrastructure.Configuration
{
    public class AlunoRespostaConfiguration : IEntityTypeConfiguration<AlunoResposta>
    {
        public void Configure(EntityTypeBuilder<AlunoResposta> builder)
        {
            builder.ToTable("AlunoResposta");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Resposta).HasMaxLength(500).IsRequired();
            builder.HasIndex(e => new { e.IdAluno, e.IdPergunta }).IsUnique();
            builder.HasOne(e => e.Aluno)
                  .WithMany(a => a.Respostas)
                  .HasForeignKey(e => e.IdAluno)
                  .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.Pergunta)
                  .WithMany(p => p.Respostas)
                  .HasForeignKey(e => e.IdPergunta)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
