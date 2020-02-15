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
    public class AssetMapperTests
    {
        [Fact]
        public void CanMapBizDtoToApiDto()
        {
            var asset = new AssetDto()
            {
                Id = Guid.NewGuid(),
                Hostname = "server1",
                Rack = new RackDto()
                {
                    Id = Guid.NewGuid(),
                    RowLetter = "A",
                    RackNumber = 1
                },
                Model = new ModelDto()
                {
                    Id = Guid.NewGuid(),
                    Vendor = "foo vendor",
                    ModelNumber = "foo number",
                    Height = 4,
                    DisplayColor = "#0f0f0f"
                },
                Owner = new UserDto()
                {
                    Id = Guid.NewGuid(),
                    Username = "fooBarBazUsername"
                }
            };

            var apiDto = asset.MapTo<GetAssetApiDto>();

            apiDto.Should()
                .NotBeNull();

            apiDto.Should().BeEquivalentTo(asset, options => 
                options.Excluding(o => o.Model)
                    .Excluding(o => o.Owner));

        }
    }
}
