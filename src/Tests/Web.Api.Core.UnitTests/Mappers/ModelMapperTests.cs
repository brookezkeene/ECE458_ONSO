using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Web.Api.Core.Dtos;
using Web.Api.Core.UnitTests.Common;
using Web.Api.Dtos;
using Web.Api.Dtos.Models.Create;
using Web.Api.Dtos.Models.Read;
using Web.Api.Dtos.Models.Update;
using Web.Api.Mappers;
using Xunit;
using Xunit.Abstractions;

namespace Web.Api.Core.UnitTests.Mappers
{
    public class ModelMapperTests : IClassFixture<BaseMapperFixture>
    {
        private readonly Guid _modelId = Guid.NewGuid();
        private readonly Guid _networkId1 = Guid.NewGuid();
        private readonly Guid _networkId2 = Guid.NewGuid();
        private readonly IMapper _mapper;

        public ModelMapperTests(BaseMapperFixture fixture)
        {
            _mapper = fixture.GetMapper();
        }

        [Fact]
        public void CanMapModelDto_ToGetModelApiDto()
        {
            var model = BuildModel(_modelId, _networkId1, _networkId2);

            var apiDto = _mapper.Map<GetModelApiDto>(model);
            model.Should().BeEquivalentTo(apiDto);
        }

        [Fact]
        public void CanMapCreateModelApiDto_ToModelDto()
        {
            var apiDto = BuildCreateModelApiDto();

            var model = _mapper.Map<ModelDto>(apiDto);

            model.Should()
                .BeEquivalentTo(apiDto);
        }
        [Fact]
        public void CanMapUpdateModelApiDto_ToModelDto()
        {
            var apiDto = BuildUpdateModelApiDto(_modelId, _networkId1, _networkId2);

            var model = _mapper.Map<ModelDto>(apiDto);

            model.Should()
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
                Height = 4,
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

        private static CreateModelApiDto BuildCreateModelApiDto()
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
                    Hostname = "server10",
                    AssetNumber = 999999,
                },
                new AssetDto
                {
                    Id = Guid.NewGuid(),
                    Hostname = "server11",
                    AssetNumber = 000000,
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
