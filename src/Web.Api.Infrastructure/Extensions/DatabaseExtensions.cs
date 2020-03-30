using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Interfaces;

namespace Web.Api.Infrastructure.Extensions
{
    public static class DatabaseExtensions
    {
        public static void RegisterSqlServerDbContexts<TApplicationDbContext, TAuditLoggingDbContext>(this IServiceCollection services, string applicationConnectionString, string auditLoggingConnectionString)
            where TApplicationDbContext : DbContext, IApplicationDbContext
            where TAuditLoggingDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
        {
            var migrationsAssembly = typeof(DatabaseExtensions).GetTypeInfo()
                .Assembly.GetName()
                .Name;

            // application and identity connection
            services.AddDbContext<TApplicationDbContext>(options =>
                options.UseLazyLoadingProxies()
                    .UseSqlServer(applicationConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));

            // audit logging connection
            services.AddDbContext<TAuditLoggingDbContext>(options =>
                options.UseSqlServer(auditLoggingConnectionString, sql => sql.MigrationsAssembly(migrationsAssembly)));
        }

        public static void DeletePreviousPowerConnections(this IApplicationDbContext context)
        {
            var oldConnections = context.PowerConnections
                .Where(conn => conn.Ports.Count != 2);

            context.PowerConnections.RemoveRange(oldConnections);

            context.SaveChanges();
        }

        public static void DeletePreviousNetworkConnections(this IApplicationDbContext context)
        {
            var oldConnections = context.NetworkConnections
                .Where(conn => conn.Ports.Count != 2);

            context.NetworkConnections.RemoveRange(oldConnections);

            context.SaveChanges();
        }
    }
}
