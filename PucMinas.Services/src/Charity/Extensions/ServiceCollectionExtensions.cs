using Microsoft.Extensions.DependencyInjection;
using PucMinas.Services.Charity.Application;

namespace PucMinas.Services.Charity.Extensions
{
    public static class ServiceCollectionExtensions
    {
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
            });

            return services;
        }

        public static IServiceCollection AddApplications(this IServiceCollection services)
        {
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
