using ApiProvaOnline.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApiProvaOnline.Infrastructure.Configuration
{
    public class AdministrativoConfiguration : IEntityTypeConfiguration<Administrativo>
    {
        public void Configure(EntityTypeBuilder<Administrativo> builder)
        {
            builder.ToTable("Administrativo");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Nome).HasMaxLength(150).IsRequired();
            builder.Property(e => e.Email).HasMaxLength(150).IsRequired();
            builder.Property(e => e.Senha).HasMaxLength(150).IsRequired();
            builder.Property(e => e.DataHoraRegistro).HasColumnName("DataHoraRegistro");
        }
    }
}
