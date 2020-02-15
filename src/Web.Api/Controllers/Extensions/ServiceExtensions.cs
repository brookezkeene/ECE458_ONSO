using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Web.Api.Configuration.Constants;
using Web.Api.Core.Auth;
using Web.Api.Core.Auth.Interfaces;
using Web.Api.Core.Services;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Helpers;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Extensions;
using Web.Api.Infrastructure.Repositories;
using Web.Api.Infrastructure.Repositories.Interfaces;
using Web.Api.Resources;
using System.Threading.Tasks;

namespace Web.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection ConfigureCoreServices(this IServiceCollection services)
        {
            // repositories
            services.ConfigureRepositories();

            // services
            services.AddTransient<IModelService, ModelService>();
            services.AddTransient<IInstanceService, InstanceService>();
            services.AddTransient<IIdentityService, IdentityService>();
            services.AddTransient<IRackService, RackService>();

            // etc
            services.ConfigureResources();

            return services;
        }

        public static IServiceCollection ConfigureRepositories(this IServiceCollection services)
        {
            services.AddTransient<IModelRepository, ModelRepository>();
            services.AddTransient<IRackRepository, RackRepository>();
            services.AddTransient<IInstanceRepository, InstanceRepository>();
            services.AddTransient<IIdentityRepository, IdentityRepository>();

            return services;
        }

        public static IServiceCollection ConfigureResources(this IServiceCollection services)
        {
            services.AddScoped<IApiErrorResources, ApiErrorResources>();

            return services;
        }

        public static IServiceCollection AddApiAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJwtFactory, JwtFactory>();

            // jwt wire up
            // Get options from app settings
            var jwtAppSettingOptions = configuration.GetSection(nameof(JwtIssuerOptions));

            const string secretKey = "iNivDmHLpUA223sqsfhqGbMRdRj1PVkH"; // todo: get this from somewhere secure
            var _signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey));

            // Configure JwtIssuerOptions
            services.Configure<JwtIssuerOptions>(options =>
                {
                    options.Issuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                    options.Audience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)];
                    options.SigningCredentials = new SigningCredentials(_signingKey, SecurityAlgorithms.HmacSha256);
                });


            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)],

                ValidateAudience = true,
                ValidAudience = jwtAppSettingOptions[nameof(JwtIssuerOptions.Audience)],

                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _signingKey,

                RequireExpirationTime = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(configureOptions =>
            {
                configureOptions.ClaimsIssuer = jwtAppSettingOptions[nameof(JwtIssuerOptions.Issuer)];
                configureOptions.TokenValidationParameters = tokenValidationParameters;
                configureOptions.SaveToken = true;
            });

            // add identity
            var identityBuilder = services.AddIdentityCore<User>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });

            identityBuilder = new IdentityBuilder(identityBuilder.UserType, typeof(IdentityRole), identityBuilder.Services);
            identityBuilder.AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

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

        public static async Task<IHost> MigrateDatabase<T>(this IHost host) where T : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var db = services.GetRequiredService<T>();
                    db.Database.Migrate();
                    await DbMigrationHelper.EnsureSeedData(host);
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
