using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Web.Api.Extensions;
using Web.Api.Infrastructure.DbContexts;
using Xunit;

namespace Web.Api.Core.UnitTests.Common
{
    public class BaseMapperFixture 
    {
        public IServiceProvider Provider;

        public IMapper GetMapper(Action<IServiceCollection> configure = default)
        {
            BuildServiceProvider(configure);
            return Provider.GetRequiredService<IMapper>();
        }

        public IServiceProvider BuildServiceProvider(Action<IServiceCollection> configure = default)
        {
            Provider = TestHelpers.BuildDiContainer(services =>
            {
                configure?.Invoke(services);
                services.AddMappingConfigurations();
                services.AddInMemoryDbContexts<ApplicationDbContext, AuditLogDbContext>();
                services.AddCoreServices<ApplicationDbContext>();
            });
            return Provider;
        }
    }
}