using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Mappers;
using Web.Api.Core.Mappers.Import;
using Xunit;

namespace Web.Api.Core.UnitTests.Mappers.Core
{
    public class ImportMapperTests
    {
        [Fact]
        public void ModelImportConfigIsValid()
        {
            ImportMapper.AssertConfigurationIsValid<ModelImportMapperProfile>();
        }
    }
}
