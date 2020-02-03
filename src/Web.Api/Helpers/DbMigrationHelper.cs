using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.Api.Core.UnitTests.Mappers;
using Web.Api.Infrastructure.DbContexts;
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
            var modelRepository = scope.ServiceProvider.GetRequiredService<IModelRepository>();
            var instanceRepository = scope.ServiceProvider.GetRequiredService<IInstanceRepository>();
            var rackRepository = scope.ServiceProvider.GetRequiredService<IRackRepository>();

            EnsureSeedData(context, modelRepository, instanceRepository, rackRepository);
        }

        public static void EnsureSeedData(ApplicationDbContext context, IModelRepository modelRepository,
            IInstanceRepository instanceRepository, IRackRepository rackRepository)
        {
            if (!context.Models.Any())
            {
                // TODO: migration failing because racks do not exist; generate them too!
                var models = ModelMock.GenerateRandomModels(20);
                context.Models.AddRange(models);
                context.SaveChanges();
            }
        }
    }
}
