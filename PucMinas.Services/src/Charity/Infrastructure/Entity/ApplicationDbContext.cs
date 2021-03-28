using Microsoft.EntityFrameworkCore;
using PucMinas.Services.Charity.Domain.Models.Charitable;
using PucMinas.Services.Charity.Domain.Models.Donor;
using PucMinas.Services.Charity.Domain.Models.Login;
using PucMinas.Services.Charity.Infrastructure.Entity.Configuation;
using System.Linq;

namespace PucMinas.Services.Charity.Infrastructure.Entity
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Item> Items { get; set; }

        public DbSet<CharitableEntity> CharitableEntities { get; set; }
        public DbSet<CharitableInformation> CharitableInformations { get; set; }
        
        public DbSet<DonorPF> DonorsPF { get; set; }
        public DbSet<DonorPJ> DonorsPJ { get; set; }

        public DbSet<Donation> Donations { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public ApplicationDbContext()
        {                
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new ItemConfiguration());

            modelBuilder.ApplyConfiguration(new CharitableEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CharitableInformationConfiguration());

            modelBuilder.ApplyConfiguration(new DonationConfiguration());
            modelBuilder.ApplyConfiguration(new DonationItemConfiguration());

            modelBuilder.ApplyConfiguration(new DonorPFConfiguration());
            modelBuilder.ApplyConfiguration(new DonorPJConfiguration());

            modelBuilder.ApplyConfiguration(new ApprovalConfiguration());
            modelBuilder.ApplyConfiguration(new CharitableInformationItemConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
