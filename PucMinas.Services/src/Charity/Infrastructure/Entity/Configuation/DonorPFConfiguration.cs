using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PucMinas.Services.Charity.Domain.Models.Donor;

namespace PucMinas.Services.Charity.Infrastructure.Entity.Configuation
{
    public class DonorPFConfiguration : IEntityTypeConfiguration<DonorPF>
    {
        public void Configure(EntityTypeBuilder<DonorPF> builder)
        {
            builder.ToTable("TbDonorPF");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id").IsRequired().ValueGeneratedNever();
            builder.Property(p => p.Name).HasColumnName("Name").IsRequired().HasMaxLength(250);
            builder.Property(p => p.CPF).HasColumnName("CPF").IsRequired().HasMaxLength(20);
            builder.Property(p => p.Birthday).HasColumnType("datetime").HasColumnName("Birthday").IsRequired();
            builder.Property(p => p.Genre).HasColumnName("Genre").IsRequired().HasMaxLength(50);
                   
            builder.OwnsOne(p => p.Address).Property(p => p.Country).HasColumnName("Country").IsRequired().HasMaxLength(200); 
            builder.OwnsOne(p => p.Address).Property(p => p.City).HasColumnName("City").IsRequired().HasMaxLength(200);
            builder.OwnsOne(p => p.Address).Property(p => p.State).HasColumnName("State").IsRequired().HasMaxLength(200);

            // Ignore
            builder.OwnsOne(p => p.Address).Ignore(p => p.CEP);
            builder.OwnsOne(p => p.Address).Ignore(p => p.AddressName);
            builder.OwnsOne(p => p.Address).Ignore(p => p.Complement);
            builder.OwnsOne(p => p.Address).Ignore(p => p.District);
            builder.OwnsOne(p => p.Address).Ignore(p => p.Number);

            builder.HasOne(p => p.User).WithOne(u => u.DonorPF).HasForeignKey<DonorPF>(e => e.UserId);
        }
    }
}
