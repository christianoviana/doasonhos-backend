using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PucMinas.Services.Charity.Domain.Enums;
using PucMinas.Services.Charity.Domain.Models.Charitable;
using System;

namespace PucMinas.Services.Charity.Infrastructure.Entity.Configuation
{
    public class CharitableEntityConfiguration : IEntityTypeConfiguration<CharitableEntity>
    {
        public void Configure(EntityTypeBuilder<CharitableEntity> builder)
        {
            builder.ToTable("TbCharitableEntity");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id").IsRequired().ValueGeneratedNever();
            builder.Property(p => p.Name).HasColumnName("Name").IsRequired().HasMaxLength(250);
            builder.Property(p => p.Cnpj).HasColumnName("CNPJ").IsRequired().HasMaxLength(20);
            builder.Property(p => p.RepresentantName).HasColumnName("RepresentantName").IsRequired().HasMaxLength(150);
            builder.Property(p => p.Status).HasColumnName("Status").HasConversion(v => v.ToString(), v => (ApproverStatus) Enum.Parse(typeof(ApproverStatus), v));
            builder.Property(p => p.Approver).HasColumnName("Approver").HasMaxLength(150);
            builder.Property(p => p.ApproverData).HasColumnType("datetime").HasColumnName("ApproverData");
            builder.Property(p => p.IsActive).HasColumnName("Activated").IsRequired();

            builder.OwnsOne(p => p.ContactNumber).Property(p => p.Telephone).HasColumnName("Telephone").IsRequired();
            builder.OwnsOne(p => p.ContactNumber).Property(p => p.CellPhone).HasColumnName("CellPhone").IsRequired();

            builder.OwnsOne(p => p.Address).Property(p => p.CEP).HasColumnName("CEP").IsRequired().HasMaxLength(25);
            builder.OwnsOne(p => p.Address).Property(p => p.AddressName).HasColumnName("AddressName").IsRequired().HasMaxLength(200);
            builder.OwnsOne(p => p.Address).Property(p => p.Country).HasColumnName("Country").IsRequired().HasMaxLength(200); 
            builder.OwnsOne(p => p.Address).Property(p => p.City).HasColumnName("City").IsRequired().HasMaxLength(200);
            builder.OwnsOne(p => p.Address).Property(p => p.State).HasColumnName("State").IsRequired().HasMaxLength(200);
            builder.OwnsOne(p => p.Address).Property(p => p.District).HasColumnName("District").IsRequired().HasMaxLength(200);
            builder.OwnsOne(p => p.Address).Property(p => p.Number).HasColumnName("Number").IsRequired().HasMaxLength(15);
            builder.OwnsOne(p => p.Address).Property(p => p.Complement).HasColumnName("Complement").HasMaxLength(150);

            builder.HasOne(p => p.User).WithOne(u => u.CharitableEntity).HasForeignKey<CharitableEntity>(e => e.UserId);
        }
    }
}
