using ApiProvaOnline.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiProvaOnline.Infrastructure.Configuration
{
    public class ProvaConfiguration : IEntityTypeConfiguration<Prova>
    {
        public void Configure(EntityTypeBuilder<Prova> builder)
        {
            builder.ToTable("Prova");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Titulo).HasMaxLength(100).IsRequired();
            builder.Property(e => e.Codigo).HasMaxLength(45).IsRequired();
            builder.HasOne(e => e.Professor)
                  .WithMany(p => p.Provas)
                  .HasForeignKey(e => e.IdProfessor)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
