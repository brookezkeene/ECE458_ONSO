using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Mappers;
using Xunit;

namespace Web.Api.Core.UnitTests.Mappers
{
    public class RackMapperTests
    {
        [Fact]
        public void RackApiMapperConfigurationIsValid()
        {
            ApiMappers.AssertConfigurationIsValid<RackApiMapperProfile>();
        }
    }
}
