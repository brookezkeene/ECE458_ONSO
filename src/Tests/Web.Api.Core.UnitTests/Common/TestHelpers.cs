using System;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Web.Api.Core.UnitTests.Common
{
    public static class TestHelpers
    {
        public static ServiceProvider BuildDiContainer(Action<IServiceCollection> cfg)
        {
            var services = new ServiceCollection();
            cfg.Invoke(services);
            return services.BuildServiceProvider();
        }

        public static ServiceProvider BuildServiceProvider(this ServiceCollection services,
            Action<ServiceCollection> cfg)
        {
            cfg.Invoke(services);
            return services.BuildServiceProvider();
        }

        public static void AssertConfigurationIsValid<TProfile>(this IMapper mapper)
            where TProfile : Profile, new()
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid<TProfile>();
        }

        public static void AssertConfigurationIsValid(this IMapper mapper)
        {
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
    }
}