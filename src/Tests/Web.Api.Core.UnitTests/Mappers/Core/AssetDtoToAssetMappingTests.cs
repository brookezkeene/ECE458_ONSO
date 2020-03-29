using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using Web.Api.Configuration.Test;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Core.UnitTests.Common;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Extensions;
using Web.Api.Infrastructure.Repositories.Interfaces;
using Xunit;

namespace Web.Api.Core.UnitTests.Mappers.Core
{
    public class AssetDtoToAssetMappingTests : IClassFixture<BaseMapperFixture>
    {
        private readonly IMapper _mapper;
        private readonly AssetDto _assetDto;

        public AssetDtoToAssetMappingTests(BaseMapperFixture fixture)
        {
            _assetDto = AssetDto();
            _mapper = fixture.GetMapper(services =>
            {
                services.AddSingleton(SetupMockModelRepo(_assetDto.Model));
            });
        }

        [Fact]
        public void AssetMapper_GivenEmptyNetworkPorts_HydratesFromModelWithoutConnections()
        {
            var asset = _mapper.Map<Asset>(_assetDto);

            asset.NetworkPorts.Should()
                .HaveSameCount(_assetDto.Model.NetworkPorts);
            _assetDto.Model.NetworkPorts.ForEach(modelPort =>
            {
                asset.NetworkPorts.Should()
                    .Contain(assetPort => assetPort.ModelNetworkPortId == modelPort.Id);
            });
        }

        [Fact]
        public void AssetMapper_GivenEmptyPowerPorts_HydratesFromModelWithoutConnections()
        {
            var asset = _mapper.Map<Asset>(_assetDto);

            var numPowerPorts = _assetDto.Model.PowerPorts.GetValueOrDefault();
            asset.PowerPorts.Should()
                .HaveCount(numPowerPorts);
            Enumerable.Range(1, numPowerPorts).ToList().ForEach(portNumber =>
            {
                asset.PowerPorts.Should()
                    .Contain(port => port.Number == portNumber);
            });
        }

        private static IModelRepository SetupMockModelRepo(ModelDto modelDto)
        {
            var modelNetworkPortIds = modelDto.NetworkPorts.Select(port => port.Id);
            var model = new Model
            {
                NetworkPorts = modelNetworkPortIds.Select(id => new ModelNetworkPort
                {
                    Id = id
                }).ToList(),
                PowerPorts = modelDto.PowerPorts
            };

            var modelRepo = new Mock<IModelRepository>();
            modelRepo.Setup(o => o.GetModel(It.IsAny<Guid>()))
                .Returns(model);

            return modelRepo.Object;
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
                DisplayColor = "#0f0f0f",
                NetworkPorts = new List<ModelNetworkPortDto>
                {
                    new ModelNetworkPortDto
                    {
                        Id = Guid.NewGuid(),
                        Name = "1",
                        Number = 1
                    },
                    new ModelNetworkPortDto
                    {
                        Id = Guid.NewGuid(),
                        Name = "2",
                        Number = 2
                    }
                },
                EthernetPorts = 2,
                PowerPorts = 2
            };
            var owner = new UserDto()
            {
                Id = Guid.NewGuid(),
                Username = "fooBarBazUsername"
            };

            var networkPorts = new List<AssetNetworkPortDto>()
            {
            };

            var powerPorts = new List<AssetPowerPortDto>()
            {
            };

            var asset = new AssetDto()
            {
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

    }
}
