using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Web.Api.Extensions;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Extensions;

namespace Web.Api.Configuration.Test
{
    public class StartupTest : Startup
    {
        public StartupTest(IConfiguration configuration) : base(configuration)
        {
        }

        public override void RegisterDbContexts(IServiceCollection services)
        {
            services.AddInMemoryDbContexts<ApplicationDbContext, AuditLogDbContext>();
        }
    }
}
