using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PucMinas.Services.Charity.Domain.Models.Login;

namespace PucMinas.Services.Charity.Infrastructure.Entity.Configuation
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("TbUserRoles");
            builder.HasKey(ur => new { ur.RoleId, ur.UserId });
            builder.HasOne<User>(ur => ur.User).WithMany(ur => ur.UserRoles).HasForeignKey(ur => ur.UserId);
            builder.HasOne<Role>(ur => ur.Role).WithMany(ur => ur.UserRoles).HasForeignKey(ur => ur.RoleId);
            builder.Property(ur => ur.UserId).HasColumnName("UserId").IsRequired();
            builder.Property(ur => ur.RoleId).HasColumnName("RoleId").IsRequired();
        }
    }
}
