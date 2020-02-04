using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.Api.Configuration;
using Web.Api.Core.UnitTests.Mappers;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Helpers
{
    public static class DbMigrationHelper
    {
        public static void EnsureSeedData(IHost host)
        {
            using var serviceScope = host.Services.CreateScope();
            var services = serviceScope.ServiceProvider;
            EnsureSeedData(services);
        }

        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var seedDataConfiguration = scope.ServiceProvider.GetRequiredService<SeedDataConfiguration>();

            EnsureSeedData(context, seedDataConfiguration);
        }

        public static void EnsureSeedData(ApplicationDbContext context, SeedDataConfiguration seedDataConfiguration)
        {
            if (!context.Racks.Any())
            {
                foreach (var rack in seedDataConfiguration.Racks)
                {
                    context.Add(rack);
                }

                context.SaveChanges();
            }

            if (!context.Models.Any())
            {
                foreach (var model in seedDataConfiguration.Models)
                {
                    context.Add(model);
                }

                context.SaveChanges();
            }

            if (!context.Instances.Any())
            {
                foreach (var instance in seedDataConfiguration.Instances)
                {
                    // prevent attempts to track entities with the same Id multiple times (causes exceptions)
                    var model = context.Set<Model>()
                        .SingleOrDefault(o => o.Id == instance.Model.Id);
                    if (model != null) instance.Model = model;

                    var rack = context.Set<Rack>()
                        .SingleOrDefault(o => o.Id == instance.Rack.Id);
                    if (rack != null) instance.Rack = rack;
                    context.Add(instance);
                }
                context.SaveChanges();
            }
        }
    }
}
