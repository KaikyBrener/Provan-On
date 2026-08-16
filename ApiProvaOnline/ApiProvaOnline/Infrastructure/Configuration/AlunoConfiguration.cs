using ApiProvaOnline.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiProvaOnline.Infrastructure.Configuration
{
    public class AlunoConfiguration : IEntityTypeConfiguration<Aluno>
    {
        public void Configure(EntityTypeBuilder<Aluno> builder)
        {
            builder.ToTable("Aluno");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Nome).HasMaxLength(100).IsRequired();
            builder.HasOne(e => e.Prova)
                  .WithMany(p => p.Alunos)
                  .HasForeignKey(e => e.IdProva)
                  .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
