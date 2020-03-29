using System;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Skoruba.AuditLogging.EntityFramework.DbContexts;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.AuditLogging.EntityFramework.Extensions;
using Skoruba.AuditLogging.EntityFramework.Repositories;
using Skoruba.AuditLogging.EntityFramework.Services;
using Web.Api.Configuration.Constants;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Extensions;
using Web.Api.Infrastructure.Interfaces;
using Web.Api.Infrastructure.Repositories;
using Web.Api.Infrastructure.Repositories.Interfaces;
using Web.Api.Resources;

namespace Web.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAuditEventLogging<TAuditLoggingDbContext, TAuditLog>(this IServiceCollection services)
            where TAuditLog : AuditLog, new()
            where TAuditLoggingDbContext : IAuditLoggingDbContext<TAuditLog>
        {
            services.AddAuditLogging(options =>
                {
                    options.UseDefaultAction = true;
                    options.UseDefaultSubject = true;
                    options.Source = "Web";
                })
                .AddDefaultHttpEventData(subjectOptions =>
                {
                    subjectOptions.SubjectIdentifierClaim = ClaimTypes.NameIdentifier;
                    subjectOptions.SubjectNameClaim = ClaimTypes.Name;
                })
                .AddAuditSinks<DatabaseAuditEventLoggerSink<TAuditLog>>();

            services
                .AddTransient<IAuditLoggingRepository<TAuditLog>,
                    AuditLoggingRepository<TAuditLoggingDbContext, TAuditLog>>();

            return services;
        }
        public static IServiceCollection AddCoreServices<TApplicationDbContext>(this IServiceCollection services)
            where TApplicationDbContext : DbContext, IApplicationDbContext
        {
            // repositories
            services.TryAddTransient<IModelRepository, ModelRepository<TApplicationDbContext>>();
            services.TryAddTransient<IRackRepository, RackRepository<TApplicationDbContext>>();
            services.TryAddTransient<IAssetRepository, AssetRepository<TApplicationDbContext>>();
            services.TryAddTransient<IIdentityRepository, IdentityRepository<TApplicationDbContext>>();
            services.TryAddTransient<IDatacenterRepository, DatacenterRepository<TApplicationDbContext>>();
            services.TryAddTransient<IImportRepository, ImportRepository<TApplicationDbContext>>();
            services.TryAddTransient<INetworkRepository, NetworkRepository<TApplicationDbContext>>();
            services.TryAddTransient<IPowerRepository, PowerRepository<TApplicationDbContext>>();
            services.TryAddTransient<IChangePlanRepository, ChangePlanRepository<TApplicationDbContext>>();

            // services
            services.TryAddTransient<IModelService, ModelService>();
            services.TryAddTransient<IAssetService, AssetService>();
            services.TryAddTransient<IIdentityService, IdentityService>();
            services.TryAddTransient<IRackService, RackService>();
            services.TryAddTransient<IDatacenterService, DatacenterService>();
            services.TryAddTransient<IAuditLogService, AuditLogService>();
            services.TryAddTransient<IModelImportService, ModelImportService>();
            services.TryAddTransient<INetworkService, NetworkService>();
            services.TryAddTransient<IPowerService, PowerService>();
            services.TryAddTransient<IAssetImportService, AssetImportService>();
            services.TryAddTransient<IChangePlanService, ChangePlanService>();

            services.AddHttpClient<PowerStateService>();

            // resources
            services.AddScoped<IApiErrorResources, ApiErrorResources>();

            return services;
        }

        public static IServiceCollection AddApiAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                    options.LoginPath = "/signin";
                    options.LogoutPath = "/signout";
            }).AddDuke("Duke", "Duke", options =>
            {
                options.ClientId = "determined-shannon";
                options.ClientSecret = "nAMi1*c6pF26mJFrBf3QY+IQU7crZCXaWxu=rmYFbAkT$dFWez";
            });

            // add identity
            var identityBuilder = services.AddIdentity<User, IdentityRole>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });

            identityBuilder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddMappingConfigurations(this IServiceCollection services)
        {
            return services.AddAutoMapper(cfg => { cfg.Advanced.AllowAdditiveTypeMapCreation = true; },
                typeof(Startup).Assembly, typeof(DomainMapperProfile).Assembly);
        } 

        public static IServiceCollection AddDbContexts<TApplicationDbContext, TAuditLoggingDbContext>(this IServiceCollection services, IConfiguration configuration)
            where TApplicationDbContext : DbContext, IApplicationDbContext
            where TAuditLoggingDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
        {
            var connectionString =
                configuration.GetConnectionString(ConfigurationConsts.ApplicationDbConnectionStringKey);

            // audit logs are stored in the same database as the rest of the application & identity entities
            services.RegisterSqlServerDbContexts<TApplicationDbContext, TAuditLoggingDbContext>(connectionString, connectionString);

            return services;
        }

        public static IServiceCollection AddInMemoryDbContexts<TApplicationDbContext, TAuditLoggingDbContext>(this IServiceCollection services)
            where TApplicationDbContext : DbContext, IApplicationDbContext
            where TAuditLoggingDbContext : DbContext, IAuditLoggingDbContext<AuditLog>
        {
            var applicationDatabaseName = Guid.NewGuid().ToString();
            var auditLoggingDatabaseName = Guid.NewGuid().ToString();

            services.AddDbContext<TApplicationDbContext>(options => options.UseInMemoryDatabase(applicationDatabaseName));
            services.AddDbContext<TAuditLoggingDbContext>(options => options.UseInMemoryDatabase(auditLoggingDatabaseName));

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
