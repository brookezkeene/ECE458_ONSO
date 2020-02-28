using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Skoruba.AuditLogging.EntityFramework.DbContexts.Default;
using Web.Api.Extensions;
using Web.Api.Helpers;
using Web.Api.Infrastructure.DbContexts;

namespace Web.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.MigrateDatabase<ApplicationDbContext>();
            await host.MigrateDatabase<DefaultAuditLoggingDbContext>();
            await host.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
