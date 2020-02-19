using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Web.Api.Core.Dtos;
using Web.Api.Dtos;
using Web.Api.Mappers;
using Xunit;

namespace Web.Api.Core.UnitTests.Mappers
{
    public class ModelMapperTests
    {
        [Fact]
        public void ModelMapperConfigIsValid()
        {
            ApiMappers.AssertConfigurationIsValid<ModelApiMapperProfile>();
        }

        [Fact]
        public void CanMapModelDto_ToGetModelApiDto()
        {
            var model = BuildModel();

            var apiDto = model.MapTo<GetModelApiDto>();

            model.Should().BeEquivalentTo(apiDto);
        }

        private static ModelDto BuildModel()
        {
            var assets = new List<AssetDto>
            {
                new AssetDto
                {
                    Id = Guid.NewGuid(),
                    Hostname = "server10"
                },
                new AssetDto
                {
                    Id = Guid.NewGuid(),
                    Hostname = "server11"
                }
            };
            var model = new ModelDto
            {
                Id = Guid.NewGuid(),
                Vendor = "foo vendor",
                ModelNumber = "foo model number",
                Height = 4,
                DisplayColor = "#000fff",
                EthernetPorts = 2,
                PowerPorts = 2,
                Cpu = "foo cpu",
                Memory = 16,
                Storage = "foo storage",
                Comment = "foo comment",
                Assets = assets
            };

            return model;
        }
    }
}
