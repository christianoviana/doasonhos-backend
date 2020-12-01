using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PucMinas.Services.Charity.Domain.Models.Charitable;

namespace PucMinas.Services.Charity.Infrastructure.Entity.Configuation
{
    public class CharitableInformationItemConfiguration : IEntityTypeConfiguration<CharitableInformationItem>
    {
        public void Configure(EntityTypeBuilder<CharitableInformationItem> builder)
        {
            builder.ToTable("TbCharitableInformationItem");
            builder.HasKey(ci => new { ci.CharitableInformationId, ci.ItemId });
            builder.HasOne(ci => ci.CharitableInformation).WithMany(ci => ci.CharitableInformationItem).HasForeignKey(d => d.CharitableInformationId);
            builder.HasOne(ci => ci.Item).WithMany(i => i.CharitableInformationItem).HasForeignKey(i => i.ItemId);
            builder.Property(ci => ci.CharitableInformationId).HasColumnName("CharitableInformationId").IsRequired();
            builder.Property(ci => ci.ItemId).HasColumnName("ItemId").IsRequired();
        }
    }
}
