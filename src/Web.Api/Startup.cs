using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Skoruba.AuditLogging.Constants;
using Skoruba.AuditLogging.EntityFramework.Extensions;
using VueCliMiddleware;
using Web.Api.Configuration;
using Web.Api.Configuration.Constants;
using Web.Api.Extensions;
using Web.Api.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Web.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCoreServices()
                //.ConfigureInMemoryDbContext();
                .ConfigureSqlDbContext(Configuration);

            services.AddApiAuthentication(Configuration);
              
            services.AddSpaStaticFiles(options => options.RootPath = "VueClient/dist");

            services.AddControllers()
                .AddNewtonsoftJson()
                .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo {Title = "Hyposoft API", Version = "v1"});
            });
            services.AddSwaggerGenNewtonsoftSupport();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddAuditLogging(options =>
                {
                    options.UseDefaultAction = true;
                    options.UseDefaultSubject = true;
                })
                .AddDefaultHttpEventData(subjectOptions =>
                {
                    subjectOptions.SubjectIdentifierClaim = ClaimsConsts.Sub;
                    subjectOptions.SubjectNameClaim = ClaimsConsts.Name;
                })
                .AddDefaultStore(options => options.UseSqlServer(
                    Configuration.GetConnectionString(ConfigurationConsts.ApplicationDbConnectionStringKey), sql =>
                        sql.MigrationsAssembly(typeof(DatabaseExtensions).GetTypeInfo()
                            .Assembly.GetName()
                            .Name)))
                .AddDefaultAuditSink();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSpaStaticFiles();

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((document, request) =>
                {
                    var paths = document.Paths.ToDictionary(item => item.Key.ToLowerInvariant(), item => item.Value);
                    document.Paths.Clear();
                    foreach (var (key, value) in paths)
                    {
                        document.Paths.Add(key, value);
                    }
                });
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hyposoft API v1");
            });

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapToVueCliProxy(
                    "{*path}",
                    new SpaOptions { SourcePath = "VueClient" },
                    npmScript: System.Diagnostics.Debugger.IsAttached ? "serve" : null,
                    regex: "Compiled successfully",
                    forceKill: true
                    );
            });
        }

    }
}
