using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using PucMinas.Services.Charity.Configuration;
using PucMinas.Services.Charity.Domain.Interfaces;
using PucMinas.Services.Charity.Domain.Mappers;
using PucMinas.Services.Charity.Extensions;
using PucMinas.Services.Charity.Filters;
using PucMinas.Services.Charity.Infrastructure.Entity;
using PucMinas.Services.Charity.Infrastructure.Repository;
using PucMinas.Services.Charity.Infrastructure.Repository.Interfaces;
using PucMinas.Services.Charity.Middleware;
using PucMinas.Services.Charity.Services;
using System;
using System.IO;
using System.Text;

namespace Charity
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc
                    (o => {
                        o.ReturnHttpNotAcceptable = true;                        
                    })     
                    .ConfigureApiBehaviorOptions(o => {
                        o.InvalidModelStateResponseFactory = ac => new BadRequestObjectResult(ac.ModelState.ToErrorMessage());
                    })
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.ReportApiVersions = true;
            });

            JwtSettings jwtSettings = new JwtSettings();
            var jwtSettingsSection = Configuration.GetSection("JwtSettings");
            jwtSettingsSection.Bind(jwtSettings);

            services.Configure<JwtSettings>(jwtSettingsSection);
            services.AddAutoMapper(typeof(ApplicationProfile));

            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(Configuration.GetConnectionString("DoasonhoConnection"), opt => opt.CommandTimeout(60)));

            services.AddApplications();

            services.AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>));
            services.AddScoped(typeof(IPagedRepositoryAsync<>), typeof(PagedRepositoryAsync<>));
            services.AddSingleton<IJwtService, JwtService>();
            services.AddTransient(typeof(LinkFilter));

            services.AddAuthorizationWithPolicies();

            services.AddAuthentication(o =>
            {
                o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ClockSkew = TimeSpan.FromSeconds(0),
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources\Items")),
                RequestPath = new PathString("/resources/items")
            });

            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources\Charities")),
                RequestPath = new PathString("/resources/charities")
            });

            app.UseCors(o =>
            {
                o.AllowAnyOrigin();
                o.AllowAnyMethod();
                o.AllowAnyHeader();
            });

            app.UseMiddleware(typeof(ExceptionMiddleware));
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();        
        }
    }
}
