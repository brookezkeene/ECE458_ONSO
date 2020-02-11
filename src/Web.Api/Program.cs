using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Web.Api.Extensions;
using Web.Api.Helpers;
using Web.Api.Infrastructure.DbContexts;

namespace Web.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var build = CreateHostBuilder(args).Build();
            var migrate = await build.MigrateDatabase<ApplicationDbContext>();
            await migrate.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    config.AddJsonFile("seeddata.json", optional: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
