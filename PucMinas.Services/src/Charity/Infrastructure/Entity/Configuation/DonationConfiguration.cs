using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PucMinas.Services.Charity.Domain.Models.Donor;

namespace PucMinas.Services.Charity.Infrastructure.Entity.Configuation
{
    public class DonationConfiguration : IEntityTypeConfiguration<Donation>
    {
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            builder.ToTable("TbDonation");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id").IsRequired().ValueGeneratedNever();
            builder.Property(p => p.Date).HasColumnType("datetime").HasColumnName("Date").IsRequired();
            builder.Property(p => p.Total).HasColumnName("Total").IsRequired();
            builder.Property(p => p.Completed).HasColumnName("Completed").IsRequired();
            builder.Property(p => p.Canceled).HasColumnName("Canceled").IsRequired();

            builder.HasOne(p => p.User).WithMany(u => u.Donations).HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Restrict);
            builder.HasOne(p => p.CharitableEntity).WithMany(u => u.Donations).HasForeignKey(e => e.CharitableEntityId).OnDelete(DeleteBehavior.Restrict);
        }
    }
}
