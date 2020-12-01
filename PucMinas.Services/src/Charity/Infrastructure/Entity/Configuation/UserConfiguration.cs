using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PucMinas.Services.Charity.Domain.Enums;
using PucMinas.Services.Charity.Domain.Models.Login;
using System;

namespace PucMinas.Services.Charity.Infrastructure.Entity.Configuation
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("TbUser");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id").IsRequired().ValueGeneratedNever();
            builder.Property(p => p.Login).HasColumnName("Login").IsRequired().HasMaxLength(100);
            builder.Property(p => p.Password).HasColumnName("Password").IsRequired();
            builder.Property(p => p.ActivationCode).HasColumnName("ActivationCode").IsRequired();
            builder.Property(p => p.IsActive).HasColumnName("Activated").IsRequired();
            builder.Property(p => p.Type).HasColumnName("Type").IsRequired().HasConversion(v => v.ToString(), v => (LoginType)Enum.Parse(typeof(LoginType), v));
            builder.HasIndex(p => p.Login).IsUnique();
        }
    }
}
