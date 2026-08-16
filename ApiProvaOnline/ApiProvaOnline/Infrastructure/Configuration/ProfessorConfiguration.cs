using ApiProvaOnline.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiProvaOnline.Infrastructure.Configuration
{
    public class ProfessorConfiguration : IEntityTypeConfiguration<Professor>
    {
        public void Configure(EntityTypeBuilder<Professor> builder)
        {
            builder.ToTable("Professor");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Nome).HasMaxLength(150).IsRequired();
            builder.Property(e => e.Email).HasMaxLength(150).IsRequired();
            builder.Property(e => e.Senha).HasMaxLength(150).IsRequired();
        }
    }
}
