using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;

namespace Charity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration((host, config) =>
                   {
                       config.SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
                       var env = host.HostingEnvironment;

                       config.AddJsonFile("appsettings.json", false, true);
                       config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
                       config.AddEnvironmentVariables();
                   })
                   .UseStartup<Startup>();
    }
}
