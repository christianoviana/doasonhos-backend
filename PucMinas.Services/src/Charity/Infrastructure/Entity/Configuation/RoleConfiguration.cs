using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PucMinas.Services.Charity.Domain.Models.Login;

namespace PucMinas.Services.Charity.Infrastructure.Entity.Configuation
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("TbRole");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id").IsRequired().ValueGeneratedNever();
            builder.Property(p => p.Name).HasColumnName("Name").IsRequired().HasMaxLength(50);
            builder.Property(p => p.Description).HasColumnName("Description").HasMaxLength(250);
        }
    }
}
