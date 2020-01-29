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
        public static async Task EnsureSeedData(IHost host)
        {
            using (var serviceScope = host.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                await EnsureSeedData(services);
            }
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                //var context = new ApplicationDbContext(options);
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var modelRepository = scope.ServiceProvider.GetRequiredService<IModelRepository>();
                var instanceRepository = scope.ServiceProvider.GetRequiredService<IInstanceRepository>();
                var rackRepository = scope.ServiceProvider.GetRequiredService<IRackRepository>();

                await EnsureSeedData(context, modelRepository, instanceRepository, rackRepository);

            }
        }

        public static async Task EnsureSeedData(ApplicationDbContext context, IModelRepository modelRepository,
            IInstanceRepository instanceRepository, IRackRepository rackRepository)
        {
            if (!context.Models.Any())
            {

                //Randomizer.Seed = new Random(1337);
                var models = ModelMock.GenerateRandomModels(20);
                await context.Models.AddRangeAsync(models);

                await context.SaveChangesAsync();
            }
        }
    }
}
