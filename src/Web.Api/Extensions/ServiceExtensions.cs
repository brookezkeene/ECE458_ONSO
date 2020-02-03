using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Web.Api.Configuration.Constants;
using Web.Api.Controllers;
using Web.Api.Core;
using Web.Api.Core.Services;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Helpers;
using Web.Api.Infrastructure;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Extensions;
using Web.Api.Infrastructure.Repositories;
using Web.Api.Infrastructure.Repositories.Interfaces;
using Web.Api.Resources;

namespace Web.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureCoreServices(this IServiceCollection services)
        {
            // repositories
            services.AddTransient<IModelRepository, ModelRepository>();
            services.AddTransient<IRackRepository, RackRepository>();
            services.AddTransient<IInstanceRepository, InstanceRepository>();

            // services
            services.AddTransient<IModelService, ModelService>();

            // etc
            services.AddScoped<IApiErrorResources, ApiErrorResources>();

            return services;
        }

        public static IServiceCollection ConfigureInMemoryDbContext(this IServiceCollection services)
        {
            var databaseName = Guid.NewGuid()
                .ToString();

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(databaseName));

            return services;
        }

        public static IServiceCollection ConfigureSqlDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString =
                configuration.GetConnectionString(ConfigurationConsts.ApplicationDbConnectionStringKey);

            services.RegisterSqlServerDbContexts(connectionString);

            return services;
        }

        public static IHost MigrateDatabase<T>(this IHost host) where T : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<T>();
                    db.Database.Migrate();
                    DbMigrationHelper.EnsureSeedData(host);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            return host;
        }
    }
}
