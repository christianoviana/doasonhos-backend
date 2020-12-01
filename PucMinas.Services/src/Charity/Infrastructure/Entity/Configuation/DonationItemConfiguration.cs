using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PucMinas.Services.Charity.Domain.Models.Donor;

namespace PucMinas.Services.Charity.Infrastructure.Entity.Configuation
{
    public class DonationItemConfiguration : IEntityTypeConfiguration<DonationItem>
    {
        public void Configure(EntityTypeBuilder<DonationItem> builder)
        {
            builder.ToTable("TbDonationItem");
            builder.HasKey(di => new { di.DonationId, di.ItemId });
            builder.HasOne(d => d.Donation).WithMany(di => di.DonationItem).HasForeignKey(d => d.DonationId);
            builder.HasOne(i => i.Item).WithMany(di => di.DonationItem).HasForeignKey(i => i.ItemId);
            builder.Property(di => di.Quantity).HasColumnName("Quantity").IsRequired();
            builder.Property(di => di.DonationId).HasColumnName("DonationId").IsRequired();
            builder.Property(di => di.ItemId).HasColumnName("ItemId").IsRequired();
        }
    }
}
