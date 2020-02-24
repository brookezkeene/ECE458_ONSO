using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Mappers;
using Xunit;

namespace Web.Api.Core.UnitTests.Mappers.Core
{
    public class DomainMapperTests
    {
        [Fact]
        public void ConfigurationIsValid()
        {
            DomainMappers.AssertConfigurationIsValid<DomainMapperProfile>();
        }
    }
}
