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
        //[Fact]
        public void AssetMapperConfigIsValid()
        {
            ApiMappers.AssertConfigurationIsValid<AssetApiMapperProfile>();
        }

        //[Fact]
        public void CanMapAssetDto_ToGetAssetApiDto()
        {
            var asset = GetAssetDto();

            var apiDto = asset.MapTo<GetAssetApiDto>();

            // map didn't outright fail
            apiDto.Should()
                .NotBeNull();
            // scalar properties match
            apiDto.Hostname.Should().Be(asset.Hostname);
            apiDto.Height.Should().Be(asset.Model.Height);
            apiDto.RackPosition.Should().Be(asset.RackPosition);
            apiDto.Comment.Should().Be(asset.Comment);
            apiDto.SlotsOccupied.Should().BeEquivalentTo(asset.SlotsOccupied);

            // flattened data from complex types
            apiDto.Rack.Should().Be(asset.Rack.Address);
            apiDto.Owner.Should().Be(asset.Owner.Username);
            apiDto.Vendor.Should().Be(asset.Model.Vendor);
            apiDto.ModelNumber.Should().Be(asset.Model.ModelNumber);

            apiDto.RackId.Should()
                .Be(asset.RackId)
                .And
                .Be(asset.Rack.Id);
            apiDto.ModelId.Should()
                .Be(asset.ModelId)
                .And
                .Be(asset.Model.Id);
            apiDto.OwnerId.Should()
                .Be(asset.OwnerId)
                .And
                .Be(asset.Owner.Id);
        }

        //[Fact]
        public void CanMapCreateAssetApiDto_ToAssetDto()
        {
            var apiDto = new CreateAssetApiDto()
            {
                ModelId = Guid.NewGuid(),
                RackId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Hostname = "foo-hostname",
                RackPosition = 20,
                Comment = "this is a comment"
            };

            var asset = apiDto.MapTo<AssetDto>();

            asset.Should().NotBeNull();

            asset.Should()
                .BeEquivalentTo(apiDto);
        }

        //[Fact]
        public void CanMapUpdateAssetApiDto_ToAssetDto()
        {
            var apiDto = new UpdateAssetApiDto()
            {
                Id = Guid.NewGuid(),
                ModelId = Guid.NewGuid(),
                RackId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Hostname = "foo-hostname",
                RackPosition = 20,
                Comment = "this is a comment"
            };

            var asset = apiDto.MapTo<AssetDto>();

            asset.Should().NotBeNull();

            asset.Should()
                .BeEquivalentTo(apiDto);
        }

        private static AssetDto GetAssetDto()
        {
            var rack = new RackDto()
            {
                Id = Guid.NewGuid(),
                RowLetter = "A",
                RackNumber = 1
            };
            var model = new ModelDto()
            {
                Id = Guid.NewGuid(),
                Vendor = "foo vendor",
                ModelNumber = "foo number",
                Height = 4,
                DisplayColor = "#0f0f0f"
            };
            var owner = new UserDto()
            {
                Id = Guid.NewGuid(),
                Username = "fooBarBazUsername"
            };
            var asset = new AssetDto()
            {
                Id = Guid.NewGuid(),
                Hostname = "server1",
                RackPosition = 39,
                Comment = "important commentary",
                RackId = rack.Id,
                Rack = rack,
                ModelId = model.Id,
                Model = model,
                OwnerId = owner.Id,
                Owner = owner
            };
            return asset;
        }
    }
}
