using ApiProvaOnline.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiProvaOnline.Infrastructure.Configuration
{
    public class AlunoRespostaObjetivaConfiguration : IEntityTypeConfiguration<AlunoRespostaObjetiva>
    {
        public void Configure(EntityTypeBuilder<AlunoRespostaObjetiva> builder)
        {
            builder.ToTable("AlunoRespostaObjetiva");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Resposta).HasColumnType("char(1)").IsRequired();
            builder.Property(e => e.DataHorRegistro).HasColumnName("DataHoraRegistro");
            builder.HasIndex(e => new { e.IdAluno, e.IdPerguntaObjetiva }).IsUnique();
            builder.HasOne(e => e.Aluno)
                  .WithMany(a => a.RespostasObjetivas)
                  .HasForeignKey(e => e.IdAluno)
                  .OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(e => e.PerguntaObjetiva)
                  .WithMany(p => p.RespostasObjetivas)
                  .HasForeignKey(e => e.IdPerguntaObjetiva)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
