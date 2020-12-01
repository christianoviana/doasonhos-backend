using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PucMinas.Services.Charity.Domain.Models.Charitable;

namespace PucMinas.Services.Charity.Infrastructure.Entity.Configuation
{
    public class CharitableInformationConfiguration : IEntityTypeConfiguration<CharitableInformation>
    {
        public void Configure(EntityTypeBuilder<CharitableInformation> builder)
        {
            builder.ToTable("TbCharitableInformation");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id").IsRequired().ValueGeneratedNever();

            builder.Property(p => p.Nickname).HasColumnName("Nickname").IsRequired().HasMaxLength(250);
            builder.Property(p => p.About).HasColumnName("About").IsRequired().HasMaxLength(500);
            builder.Property(p => p.Goal).HasColumnName("Goal").IsRequired().HasMaxLength(500);

            builder.Property(p => p.ManagerDescription).HasColumnName("ManagerDescription").IsRequired().HasMaxLength(500);
            builder.Property(p => p.TransparencyDescription).HasColumnName("TransparencyDescription").IsRequired().HasMaxLength(500);

            builder.Property(p => p.ManagerDescription).HasColumnName("Vision").IsRequired().HasMaxLength(500);
            builder.Property(p => p.TransparencyDescription).HasColumnName("Mission").IsRequired().HasMaxLength(500);
            builder.Property(p => p.TransparencyDescription).HasColumnName("Values").IsRequired().HasMaxLength(500);

            builder.Property(p => p.SiteUrl).HasColumnName("SiteUrl");
            builder.Property(p => p.Email).HasColumnName("Email");

            builder.Property(p => p.PicturePath).HasColumnName("PicturePath");
            builder.Property(p => p.PictureUrl).HasColumnName("PictureUrl");

            builder.OwnsOne(p => p.Photo01).Property(p => p.ImagePath).HasColumnName("ImagePath01").IsRequired();
            builder.OwnsOne(p => p.Photo01).Property(p => p.ImageUrl).HasColumnName("ImageUrl01").IsRequired();
            builder.OwnsOne(p => p.Photo01).Property(p => p.Title).HasColumnName("Title01").IsRequired();

            builder.OwnsOne(p => p.Photo02).Property(p => p.ImagePath).HasColumnName("ImagePath02").IsRequired();
            builder.OwnsOne(p => p.Photo02).Property(p => p.ImageUrl).HasColumnName("ImageUrl02").IsRequired();
            builder.OwnsOne(p => p.Photo02).Property(p => p.Title).HasColumnName("Title02").IsRequired();

            builder.HasOne(p => p.CharitableEntity).WithOne(u => u.CharitableInformation).HasForeignKey<CharitableInformation>(e => e.CharitableEntityId);
        }
    }
}
