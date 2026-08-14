using ApiProvaOnline.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiProvaOnline.Infrastructure.Configuration
{
    public class PerguntaConfiguration : IEntityTypeConfiguration<Pergunta>
    {
        public void Configure(EntityTypeBuilder<Pergunta> builder)
        {
            builder.ToTable("Pergunta");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Titulo).HasMaxLength(500).IsRequired();
            builder.Property(e => e.Descricao).HasMaxLength(500).IsRequired();
            builder.HasOne(e => e.Prova)
                  .WithMany(p => p.Perguntas)
                  .HasForeignKey(e => e.IdProva)
                  .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
