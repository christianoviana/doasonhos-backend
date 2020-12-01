using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PucMinas.Services.Charity.Domain.Models.Donor;

namespace PucMinas.Services.Charity.Infrastructure.Entity.Configuation
{
    public class DonorPJConfiguration : IEntityTypeConfiguration<DonorPJ>
    {
        public void Configure(EntityTypeBuilder<DonorPJ> builder)
        {
            builder.ToTable("TbDonorPJ");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id").IsRequired().ValueGeneratedNever();
            builder.Property(p => p.CompanyName).HasColumnName("CompanyName").IsRequired().HasMaxLength(250);
            builder.Property(p => p.ContactName).HasColumnName("ContactName").IsRequired().HasMaxLength(250);
            builder.Property(p => p.CNPJ).HasColumnName("CNPJ").IsRequired().HasMaxLength(20);
                   
            builder.OwnsOne(p => p.Address).Property(p => p.Country).HasColumnName("Country").IsRequired().HasMaxLength(200); 
            builder.OwnsOne(p => p.Address).Property(p => p.City).HasColumnName("City").IsRequired().HasMaxLength(200);
            builder.OwnsOne(p => p.Address).Property(p => p.State).HasColumnName("State").IsRequired().HasMaxLength(200);
            // Ignore
            builder.OwnsOne(p => p.Address).Ignore(p => p.CEP);
            builder.OwnsOne(p => p.Address).Ignore(p => p.AddressName);
            builder.OwnsOne(p => p.Address).Ignore(p => p.Complement);
            builder.OwnsOne(p => p.Address).Ignore(p => p.District);
            builder.OwnsOne(p => p.Address).Ignore(p => p.Number);

            builder.HasOne(p => p.User).WithOne(u => u.DonorPJ).HasForeignKey<DonorPJ>(e => e.UserId);
        }
    }
}
