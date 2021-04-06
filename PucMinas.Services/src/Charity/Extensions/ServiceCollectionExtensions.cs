using Microsoft.Extensions.DependencyInjection;
using PucMinas.Services.Charity.Application;
using PucMinas.Services.Charity.Infrastructure.Entity;

namespace PucMinas.Services.Charity.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void SeedDatabase(this IServiceCollection services)
        {
            ServiceProvider serviceProvider = services.BuildServiceProvider();
            var AppDbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            SeedData.InitializeDatabase(AppDbContext);
        }

        public static IServiceCollection AddAuthorizationWithPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(o =>
            {
                o.AddPolicy("administrator", p => p.RequireRole("administrator"));
                o.AddPolicy("Master", p => p.RequireRole("administrator", "manager"));
                o.AddPolicy("items_read", p => p.RequireRole("administrator", "charitable_entity"));
                
                o.AddPolicy("GetGroupItems", p => p.RequireRole("administrator", "manager", "charitable_entity"));              

                o.AddPolicy("GetCharityRestrictedById", p => p.RequireRole("administrator", "manager", "charitable_entity"));
                o.AddPolicy("GetCharityApproval", p => p.RequireRole("administrator", "manager", "charitable_entity"));
                o.AddPolicy("UpdateCharity", p => p.RequireRole("administrator", "charitable_entity"));
                o.AddPolicy("CreateCharityInfo", p => p.RequireRole("administrator", "charitable_entity"));
                o.AddPolicy("UpdateCharityInfo", p => p.RequireRole("administrator", "charitable_entity"));
                o.AddPolicy("UpdateCharityInfoItem", p => p.RequireRole("administrator", "charitable_entity"));
                o.AddPolicy("UpdateCharityInfoImage", p => p.RequireRole("administrator", "charitable_entity"));
                o.AddPolicy("DeleteCharity", p => p.RequireRole("administrator"));

                // Donor PF Controller
                o.AddPolicy("GetDonorsPF", p => p.RequireRole("administrator", "manager"));
                o.AddPolicy("GetDonorPFById", p => p.RequireRole("administrator", "donor_pf"));
                o.AddPolicy("UpdateDonorPF", p => p.RequireRole("administrator", "donor_pf"));
                o.AddPolicy("DeleteDonorPF", p => p.RequireRole("administrator"));

                // Donor PJ Controller
                o.AddPolicy("GetDonorsPJ", p => p.RequireRole("administrator", "manager"));
                o.AddPolicy("GetDonorPJById", p => p.RequireRole("administrator", "donor_pj"));
                o.AddPolicy("UpdateDonorPJ", p => p.RequireRole("administrator", "donor_pj"));
                o.AddPolicy("DeleteDonorPJ", p => p.RequireRole("administrator"));

                // Donor PJ Controller
                // Donor PF Controller
                o.AddPolicy("CreateDonationOnline", p => p.RequireRole("donor_pf", "donor_pj", "external"));
                o.AddPolicy("CreateDonationPresential", p => p.RequireRole("donor_pf", "donor_pj", "external"));
                o.AddPolicy("GetDonationById", p => p.RequireRole("donor_pf", "donor_pj", "external"));
                o.AddPolicy("GetCharityDonationById", p => p.RequireRole("charitable_entity", "administrator", "manager"));
                o.AddPolicy("GetDonorsDonationById", p => p.RequireRole("donor_pf", "donor_pj", "external"));
                o.AddPolicy("CancelDonationById", p => p.RequireRole("charitable_entity", "donor_pf", "donor_pj", "external"));
                o.AddPolicy("ApproveDonationById", p => p.RequireRole("charitable_entity"));

                // Reports
                o.AddPolicy("GetUserReport", p => p.RequireRole("administrator", "manager"));
                o.AddPolicy("GetCharityReport", p => p.RequireRole("charitable_entity", "administrator", "manager"));
            });

            return services;
        }

        public static IServiceCollection AddApplications(this IServiceCollection services)
        {
            services.AddScoped(typeof(DonationApplication));
            services.AddScoped(typeof(CharitableInformationApplication));
            services.AddScoped(typeof(CharitableEntityApplication));
            services.AddScoped(typeof(DonorPJApplication));
            services.AddScoped(typeof(DonorPFApplication));
            services.AddScoped(typeof(ItemApplication));
            services.AddScoped(typeof(GroupApplication));
            services.AddScoped(typeof(UserApplication));
            services.AddScoped(typeof(RoleApplication));

            return services;
        }
    }
}
