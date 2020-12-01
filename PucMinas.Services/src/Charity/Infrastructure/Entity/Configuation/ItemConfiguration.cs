using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PucMinas.Services.Charity.Domain.Models.Donor;

namespace PucMinas.Services.Charity.Infrastructure.Entity.Configuation
{
    public class ItemConfiguration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("TbItem");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id").IsRequired().ValueGeneratedNever();
            builder.Property(p => p.Name).HasColumnName("Name").IsRequired().HasMaxLength(250).IsRequired();
            builder.Property(p => p.Description).HasColumnName("Description").IsRequired().HasMaxLength(500);
            builder.Property(p => p.Price).HasColumnName("Price");
            builder.Property(p => p.ImagePath).HasColumnName("ImagePath");
            builder.Property(p => p.ImageUrl).HasColumnName("ImageUrl");
            builder.HasOne(i => i.Group).WithMany(g => g.Items).HasForeignKey(i => i.GroupId);
        }
    }
}

