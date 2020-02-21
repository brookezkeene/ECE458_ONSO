using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Web.Api.Core.Dtos;
using Web.Api.Dtos;
using Web.Api.Dtos.Models.Create;
using Web.Api.Dtos.Models.Read;
using Web.Api.Dtos.Models.Update;
using Web.Api.Mappers;
using Xunit;

namespace Web.Api.Core.UnitTests.Mappers
{
    public class ModelMapperTests
    {
        Guid modelId = Guid.NewGuid();
        Guid networkId1 = Guid.NewGuid();
        Guid networkId2 = Guid.NewGuid();
        [Fact]
        public void ModelMapperConfigIsValid()
        {
            ApiMappers.AssertConfigurationIsValid<ModelApiMapperProfile>();
        }

        [Fact]
        public void CanMapModelDto_ToGetModelApiDto()
        {
            var model = BuildModel(modelId, networkId1, networkId2);

            var apiDto = model.MapTo<GetModelApiDto>();
            model.Should().BeEquivalentTo(apiDto);
        }

        [Fact]
        public void CanMapCreateAssetApiDto_ToAssetDto()
        {
            var apiDto = BuildCreateModelApiDto();

            var asset = apiDto.MapTo<ModelDto>();

            asset.Should()
                .BeEquivalentTo(apiDto);
        }
        [Fact]
        public void CanMapUpdateAssetApiDto_ToAssetDto()
        {
            var apiDto = BuildUpdateModelApiDto(modelId, networkId1, networkId2);

            var asset = apiDto.MapTo<ModelDto>();

            asset.Should().NotBeNull();

            asset.Should()
                .BeEquivalentTo(apiDto);
        }

        private static UpdateModelApiDto BuildUpdateModelApiDto (Guid modelId, Guid netId1, Guid netId2)
        {
            var networks = new List<UpdateModelNetworkPortDto>
            {
                new UpdateModelNetworkPortDto
                {
                    Id = netId1,
                    Name = "eth0",
                    Number = 1
                },
                new UpdateModelNetworkPortDto
                {
                    Id = netId2,
                    Name = "eth1",
                    Number = 2
                }
            };
            var model = new UpdateModelApiDto
            {
                Id = modelId,
                Vendor = "foo vendor",
                ModelNumber = "foo model number",
                DisplayColor = "#000fff",
                EthernetPorts = 2,
                PowerPorts = 2,
                Cpu = "foo cpu",
                Memory = 16,
                Storage = "foo storage",
                Comment = "foo comment",
                NetworkPorts = networks
            };
            return model;
        }

        private static CreateModelApiDto BuildCreateModelApiDto ()
        {
            var networkports = new List<CreateModelNetworkPortDto>
            {
                new CreateModelNetworkPortDto
                {
                    Name = "eth0",
                    Number = 1
                },
                new CreateModelNetworkPortDto
                {
                    Name = "eth1",
                    Number = 2
                }
            };
            var model = new CreateModelApiDto
            {
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
                NetworkPorts = networkports
            };
            return model;
        }

        

        private static ModelDto BuildModel(Guid modelId, Guid netId1, Guid netId2)
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
                Id = modelId,
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
            var networks = new List<ModelNetworkPortDto>
            {
                new ModelNetworkPortDto
                {
                    Id = netId1,
                    Model = model,
                    ModelId = modelId,
                    Name = "eth0",
                    Number = 1
                },
                new ModelNetworkPortDto
                {
                    Id = netId2,
                    Model = model,
                    ModelId = modelId,
                    Name = "eth1",
                    Number = 2                
                }
            };
            model.NetworkPorts = networks;
            return model;
        }
    }
}
