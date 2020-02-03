using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Web.Api.Infrastructure.DbContexts;

namespace Web.Api.Infrastructure.Extensions
{
    public static class DatabaseExtensions
    {
        public static void RegisterSqlServerDbContexts(this IServiceCollection services, string connectionString)
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo()
                .Assembly.GetName()
                .Name;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
        }
    }
}
