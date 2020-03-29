using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Web.Api.Configuration.Test;
using Web.Api.IntegrationTests.Common;
using Xunit;

namespace Web.Api.IntegrationTests.Tests.Base
{
    public class BaseClassFixture : IClassFixture<WebApplicationFactory<StartupTest>>
    {
        protected readonly WebApplicationFactory<StartupTest> Factory;
        protected readonly HttpClient Client;

        public BaseClassFixture(WebApplicationFactory<StartupTest> factory)
        {
            Factory = factory;
            Client = factory.SetupClient(ConfigureTestServices);
        }

        public virtual void ConfigureTestServices(IServiceCollection services) { }
    }
}
