using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentAssertions;
using Newtonsoft.Json;
using Web.Api.Core.Dtos;
using Web.Api.Dtos;
using Web.Api.Dtos.Assets;
using Web.Api.Dtos.Assets.Create;
using Web.Api.Dtos.Assets.Read;
using Web.Api.Dtos.Assets.Update;
using Web.Api.Infrastructure.Entities;
using Web.Api.Mappers;
using Xunit;
using Xunit.Abstractions;

namespace Web.Api.Core.UnitTests.Mappers
{
    public class AssetMapperTests
    {
        private readonly ITestOutputHelper _testOutput;

        public AssetMapperTests(ITestOutputHelper testOutput)
        {
            _testOutput = testOutput;
        }

        [Fact]
        public void AssetMapperConfigIsValid()
        {
            ApiMappers.AssertConfigurationIsValid<AssetApiMapperProfile>();
        }

        [Fact]
        public void CanMapAssetDto_ToGetAssetApiDto()
        {
            var asset = AssetDto();

            var apiDto = asset.MapTo<GetAssetApiDto>();

            // map didn't outright fail
            apiDto.Should()
                .NotBeNull();
            // scalar properties match
            apiDto.Hostname.Should().Be(asset.Hostname);
            apiDto.RackPosition.Should().Be(asset.RackPosition);
            apiDto.Comment.Should().Be(asset.Comment);
            apiDto.SlotsOccupied.Should().BeEquivalentTo(asset.SlotsOccupied);
            apiDto.AssetNumber.Should().Be(asset.AssetNumber);

            apiDto.PowerPorts.Should()
                .Equal(asset.PowerPorts, (api, core) => api.Id == core.Id);
            apiDto.PowerPorts.Should()
                .Equal(asset.PowerPorts, (api, core) => api.Number == core.Number);
            apiDto.PowerPorts.Should()
                .Equal(asset.PowerPorts, (api, core) => api.PduPortId == core.PduPortId);
            apiDto.PowerPorts.Should()
                .Equal(asset.PowerPorts, (api, core) => api.PduPort == core.PduPort.ToString());

            apiDto.NetworkPorts.Should()
                .Equal(asset.NetworkPorts, (api, core) => api.Id == core.Id);
            apiDto.NetworkPorts.Should()
                .Equal(asset.NetworkPorts, (api, core) => api.Name == core.ModelNetworkPort.Name);
            apiDto.NetworkPorts.Should()
                .Equal(asset.NetworkPorts, (api, core) => api.MacAddress == core.MacAddress);
            apiDto.NetworkPorts.Should()
                .Equal(asset.NetworkPorts, (api, core) => api.ConnectedPortId == core.ConnectedPortId);

            apiDto.NetworkPorts.Should()
                .Equal(asset.NetworkPorts, (api, core) => api.ConnectedPort.Id == core.ConnectedPort.Id);
            apiDto.NetworkPorts.Should()
                .Equal(asset.NetworkPorts, (api, core) => api.ConnectedPort.Name == core.ConnectedPort.ModelNetworkPort.Name);
            apiDto.NetworkPorts.Should()
                .Equal(asset.NetworkPorts, (api, core) => api.ConnectedPort.Hostname == core.ConnectedPort.Asset.Hostname);
            apiDto.NetworkPorts.Should()
                .Equal(asset.NetworkPorts, (api, core) => api.ConnectedPort.AssetId == core.ConnectedPort.AssetId);
            apiDto.NetworkPorts.Should()
                .Equal(asset.NetworkPorts, (api, core) => api.ConnectedPort.MacAddress == core.ConnectedPort.MacAddress);


            // flattened data from complex types
            apiDto.Rack.Should().Be(asset.Rack.Address);
            apiDto.Owner.Should().Be(asset.Owner.Username);
            apiDto.Datacenter.Should().Be(asset.Rack.Datacenter.Name);
            apiDto.HasNetworkManagedPower.Should().Be(asset.Rack.Datacenter.HasNetworkManagedPower);
            apiDto.Vendor.Should().Be(asset.Model.Vendor);
            apiDto.ModelNumber.Should().Be(asset.Model.ModelNumber);
            apiDto.Height.Should().Be(asset.Model.Height);
            apiDto.DisplayColor.Should().Be(asset.Model.DisplayColor);

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
            apiDto.DatacenterId.Should()
                .Be(asset.Rack.DatacenterId)
                .And
                .Be(asset.Rack.Datacenter.Id);
        }

        [Fact]
        public void CanMapCreateAssetApiDto_ToAssetDto()
        {
            var apiDto = CreateAssetApiDto();

            var asset = apiDto.MapTo<AssetDto>();

            asset.Should().NotBeEquivalentTo(apiDto);
        }

        [Fact]
        public void CanMapUpdateAssetApiDto_ToAssetDto()
        {
            var apiDto = UpdateAssetApiDto();

            var asset = apiDto.MapTo<AssetDto>();

            asset.Should().NotBeNull();

            asset.Should().NotBeEquivalentTo(apiDto);
        }

        private static AssetDto AssetDto()
        {
            var datacenter = new DatacenterDto()
            {
                Id = Guid.NewGuid(),
                Name = "RTP1",
                Description = "Research Triangle Park lab 1",
                HasNetworkManagedPower = true
            };
            var rack = new RackDto()
            {
                Id = Guid.NewGuid(),
                RowLetter = "A",
                RackNumber = 1,
                Datacenter = datacenter,
                DatacenterId = datacenter.Id
            };
            rack.Pdus = new List<PduDto>
            {
                new PduDto()
                {
                    Id = Guid.NewGuid(),
                    Location = PduLocation.L,
                    Rack = rack,
                    RackId = rack.Id
                },
                new PduDto()
                {
                    Id = Guid.NewGuid(),
                    Location = PduLocation.R,
                    Rack = rack,
                    RackId = rack.Id
                }
            };
            var pduPortL24 = new PduPortDto()
            {
                Id = Guid.NewGuid(),
                Number = 24,
                Pdu = rack.Pdus.First()
            };
            var pduPortR24 = new PduPortDto()
            {
                Id = Guid.NewGuid(),
                Number = 24,
                Pdu = rack.Pdus.Last()
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

            var modelNetworkPort1 = new ModelNetworkPortDto()
            {
                Id = Guid.NewGuid(),
                Name = "e1",
                Model = model,
                ModelId = model.Id
            };
            var modelNetworkPort2 = new ModelNetworkPortDto()
            {
                Id = Guid.NewGuid(),
                Name = "e2",
                Model = model,
                ModelId = model.Id
            };

            var assetNetworkPort1 = new AssetNetworkPortDto()
            {
                Id = Guid.NewGuid(),
                MacAddress = "00:11:22:33:44:55",
                ModelNetworkPort = modelNetworkPort1,
                ModelNetworkPortId = modelNetworkPort1.Id
            };
            var assetNetworkPort2 = new AssetNetworkPortDto()
            {
                Id = Guid.NewGuid(),
                MacAddress = "aa:bb:cc:dd:ee:ff",
                ModelNetworkPort = modelNetworkPort2,
                ModelNetworkPortId = modelNetworkPort2.Id
            };

            assetNetworkPort1.ConnectedPort = assetNetworkPort2;
            assetNetworkPort2.ConnectedPort = assetNetworkPort1;
            assetNetworkPort1.ConnectedPortId = assetNetworkPort2.Id;
            assetNetworkPort2.ConnectedPortId = assetNetworkPort1.Id;

            var networkPorts = new List<AssetNetworkPortDto>()
            {
                assetNetworkPort1,
                assetNetworkPort2
            };

            var powerPorts = new List<AssetPowerPortDto>()
            {
                new AssetPowerPortDto()
                {
                    Id = Guid.NewGuid(),
                    Number = 1,
                    PduPort = pduPortL24,
                    PduPortId = pduPortL24.Id
                },
                new AssetPowerPortDto()
                {
                    Id = Guid.NewGuid(),
                    Number = 2,
                    PduPort = pduPortR24,
                    PduPortId = pduPortR24.Id
                }
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
                Owner = owner,
                NetworkPorts = networkPorts,
                PowerPorts = powerPorts,
                AssetNumber = 100000
            };

            asset.NetworkPorts.ForEach(o =>
            {
                o.Asset = asset;
                o.AssetId = asset.Id;
            }); 

            return asset;
        }
        private static CreateAssetApiDto CreateAssetApiDto()
        {
            var powerPorts = new List<CreateAssetPowerPortApiDto>
            {
                new CreateAssetPowerPortApiDto()
                {
                    Number = 1,
                    PduPortId = Guid.NewGuid()
                },
                new CreateAssetPowerPortApiDto()
                {
                    Number = 2,
                    PduPortId = Guid.NewGuid()
                }
            };

            var networkPorts = new List<CreateAssetNetworkPortApiDto>
            {
                new CreateAssetNetworkPortApiDto()
                {
                    MacAddress = "aa:bb:cc:dd:ee:ff",
                    ConnectedPortId = Guid.NewGuid()
                },
                new CreateAssetNetworkPortApiDto
                {
                    MacAddress = "00:11:22:33:44:55"
                }
            };
            var apiDto = new CreateAssetApiDto()
            {
                ModelId = Guid.NewGuid(),
                RackId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Hostname = "foo-hostname",
                RackPosition = 20,
                Comment = "this is a comment",
                PowerPorts = powerPorts,
                NetworkPorts = networkPorts,
                AssetNumber = 100000
            };
            return apiDto;
        }
        private static UpdateAssetApiDto UpdateAssetApiDto()
        {
            var powerPorts = new List<UpdateAssetPowerPortApiDto>()
            {
                new UpdateAssetPowerPortApiDto()
                {
                    Id = Guid.NewGuid(),
                    PduPortId = Guid.NewGuid()
                },
                new UpdateAssetPowerPortApiDto()
                {
                    Id = Guid.NewGuid(),
                    PduPortId = Guid.NewGuid()
                }
            };
            var networkPorts = new List<UpdateAssetNetworkPortApiDto>()
            {
                new UpdateAssetNetworkPortApiDto()
                {
                    Id = Guid.NewGuid(),
                    ConnectedPortId = Guid.NewGuid(),
                    MacAddress = "00:11:22:33:44:55",
                },
                new UpdateAssetNetworkPortApiDto()
                {
                    Id = Guid.NewGuid(),
                    ConnectedPortId = Guid.NewGuid(),
                    MacAddress = "aa:bb:cc:dd:ee:ff",
                }
            };
            var apiDto = new UpdateAssetApiDto()
            {
                Id = Guid.NewGuid(),
                ModelId = Guid.NewGuid(),
                RackId = Guid.NewGuid(),
                OwnerId = Guid.NewGuid(),
                Hostname = "foo-hostname",
                RackPosition = 20,
                Comment = "this is a comment",
                PowerPorts = powerPorts,
                NetworkPorts = networkPorts,
                AssetNumber = 100000
            };
            return apiDto;
        }
    }
}
