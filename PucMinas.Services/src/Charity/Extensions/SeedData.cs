using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PucMinas.Services.Charity.Domain.Models.Login;
using PucMinas.Services.Charity.Infrastructure.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PucMinas.Services.Charity.Extensions
{
    public static class SeedData
    {
        public static void InitializeDatabase(ApplicationDbContext context)
        {
            if (context != null)
            {
                context.Database.Migrate();
                //context.Database.EnsureCreated();

                if (!context.Roles.Any())
                {
                    List<Role> roles = new List<Role>()
                {
                    new Role(){Id = Guid.NewGuid(), Name = "donor_pf", Description = "Regra de acesso para os usuários do tipo doadores PF (Pessoa Física)"},
                    new Role(){Id = Guid.NewGuid(), Name = "donor_pj", Description = "Regra de acesso para os usuários do tipo doadores PJ (Pessoa Jurídica)"},
                    new Role(){Id = Guid.NewGuid(), Name = "manager", Description = "Regra de acesso para os usuários do tipo gerentes"},
                    new Role(){Id = Guid.NewGuid(), Name = "administrator", Description = "Regra de acesso para os usuários do tipo administradores"},
                    new Role(){Id = Guid.NewGuid(), Name = "charitable_entity", Description = "Regra de acesso para as entidades beneficentes"},
                    new Role(){Id = Guid.NewGuid(), Name = "external", Description = "Regra de acesso para usuários externos"}
                };

                    context.Roles.AddRange(roles);
                    context.SaveChanges();
                }
            }           
        } 
    }
}
