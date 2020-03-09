using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Web.Api.Configuration;
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
            var identityRepository = scope.ServiceProvider.GetRequiredService<IIdentityRepository>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            await EnsureSeedData(context, identityRepository, roleManager, userManager);
        }

        public static async Task EnsureSeedData(ApplicationDbContext context, IIdentityRepository identityRepository, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            var allRoles = new[] { "model", "asset", "power", "audit", "admin" };
            for (var i = 0; i < 5; i++)
            {
                if (!await roleManager.RoleExistsAsync(allRoles[i]))
                {
                    if (allRoles[i] != "admin")
                    {
                        await roleManager.CreateAsync(new IdentityRole(allRoles[i]));

                    } else
                    {
                        var adminRole = new IdentityRole("admin");
                        await roleManager.CreateAsync(adminRole);
                        await roleManager.AddClaimAsync(adminRole, new Claim("owner:asset", "all"));
                    }
                }
            }

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

            var adminUser = await userManager.FindByNameAsync("admin");
            if (!await userManager.IsInRoleAsync(adminUser, "admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "admin");
            }
        }
    }
}
