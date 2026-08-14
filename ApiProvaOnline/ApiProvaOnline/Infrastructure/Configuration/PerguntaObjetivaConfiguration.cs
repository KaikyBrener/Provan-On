using ApiProvaOnline.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiProvaOnline.Infrastructure.Configuration
{
    public class PerguntaObjetivaConfiguration : IEntityTypeConfiguration<PerguntaObjetiva>
    {
        public void Configure(EntityTypeBuilder<PerguntaObjetiva> builder)
        {
            builder.ToTable("PerguntaObjetiva");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Titulo).HasMaxLength(500).IsRequired();
            builder.Property(e => e.Descricao).HasMaxLength(500).IsRequired();
            builder.Property(e => e.OpcaoA).HasMaxLength(500).IsRequired();
            builder.Property(e => e.OpcaoB).HasMaxLength(500).IsRequired();
            builder.Property(e => e.OpcaoC).HasMaxLength(500).IsRequired();
            builder.Property(e => e.OpcaoD).HasMaxLength(500).IsRequired();
            builder.Property(e => e.OpcaoCorreta).HasColumnType("char(1)").IsRequired();
            builder.HasOne(e => e.Prova)
                  .WithMany(p => p.PerguntasObjetivas)
                  .HasForeignKey(e => e.IdProva)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
