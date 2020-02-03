using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Web.Api.Controllers;
using Web.Api.Core;
using Web.Api.Core.Services;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure;
using Web.Api.Infrastructure.DbContexts;
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

        public static IServiceCollection ConfigureDbContext(this IServiceCollection services)
        {
            var databaseName = Guid.NewGuid()
                .ToString();

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(databaseName));

            return services;
        }

    }
}
