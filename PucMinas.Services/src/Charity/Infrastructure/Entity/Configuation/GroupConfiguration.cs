using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PucMinas.Services.Charity.Domain.Models.Donor;

namespace PucMinas.Services.Charity.Infrastructure.Entity.Configuation
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            builder.ToTable("TbGroup");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id").IsRequired().ValueGeneratedNever();
            builder.Property(p => p.Name).HasColumnName("Name").IsRequired().HasMaxLength(150).IsRequired();
            builder.Property(p => p.Description).HasColumnName("Description").HasMaxLength(250);
        }
    }
}
