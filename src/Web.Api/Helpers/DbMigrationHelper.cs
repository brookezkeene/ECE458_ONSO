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
        public static async Task EnsureSeedData(IHost host)
        {
            using var serviceScope = host.Services.CreateScope();
            var services = serviceScope.ServiceProvider;
            await EnsureSeedData(services);
        }

        public static async Task EnsureSeedData(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var seedDataConfiguration = scope.ServiceProvider.GetRequiredService<SeedDataConfiguration>();
            var identityRepository = scope.ServiceProvider.GetRequiredService<IIdentityRepository>();

            await EnsureSeedData(context, seedDataConfiguration,identityRepository);
        }

        public static async Task EnsureSeedData(ApplicationDbContext context, SeedDataConfiguration seedDataConfiguration, IIdentityRepository identityRepository)
        {
            if (await identityRepository.FindByNameAsync("admin") == null)
            {
                var user = new User
                {
                    FirstName = "Admin",
                    LastName = "User",
                    Email = "admin@test.com",
                    UserName = "admin"
                };
                await identityRepository.CreateUserAsync(user, "@$8^5#QqsX8K");
            }
        }
    }
}
