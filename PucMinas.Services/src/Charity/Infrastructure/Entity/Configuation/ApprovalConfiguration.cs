using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PucMinas.Services.Charity.Domain.Models.Approvals;

namespace PucMinas.Services.Charity.Infrastructure.Entity.Configuation
{
    public class ApprovalConfiguration : IEntityTypeConfiguration<Approval>
    {
        public void Configure(EntityTypeBuilder<Approval> builder)
        {
            builder.ToTable("TbApproval");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("Id").IsRequired().ValueGeneratedNever();
            builder.Property(p => p.Date).HasColumnType("datetime").HasColumnName("Date").IsRequired().HasMaxLength(10);
            builder.Property(p => p.Message).HasColumnName("Message").IsRequired().HasMaxLength(250);
            builder.Property(p => p.Detail).HasColumnName("Detail").HasMaxLength(250);
            builder.Property(p => p.Status).HasColumnName("Status").IsRequired();          

            builder.HasOne(p => p.CharitableEntity).WithMany(c => c.Approvals).HasForeignKey(p => p.CharitableEntityId);
        }
    }
}
