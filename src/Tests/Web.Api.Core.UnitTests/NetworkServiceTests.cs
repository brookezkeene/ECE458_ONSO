using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.AuditLogging.Services;
using Web.Api.Configuration.Test;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Core.UnitTests.Common;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories;
using Web.Api.Infrastructure.Repositories.Interfaces;
using Xunit;
using Xunit.Abstractions;

namespace Web.Api.Core.UnitTests
{
    public class NetworkServiceTests : IClassFixture<BaseMapperFixture>
    {
        private readonly AssetDto _asset;
        private readonly IServiceProvider _provider;

        public NetworkServiceTests(BaseMapperFixture fixture)
        {
            _asset = AssetDto();
            _provider = fixture.BuildServiceProvider(services =>
            {
                services.AddSingleton(new Mock<IAuditEventLogger>().Object);
            });
        }

        [Fact]
        public async Task CreateConnection_TwoExistingPorts_Success()
        {
            using (var scope = _provider.CreateScope())
            {
                await SetupAssetData(scope);
            }

            var portOneId = _asset.NetworkPorts[0]
                .Id;
            var portTwoId = _asset.NetworkPorts[1]
                .Id;

            const string portOneUpdatedMac = "11:22:33:44:55:66";
            const string portTwoUpdatedMac = "aa:bb:cc:dd:ee:ff";

            var connectionDto = new NetworkConnectionDto
            {
                Ports = new List<AssetNetworkPortDto>
                {
                    new AssetNetworkPortDto
                    {
                        Id = portOneId,
                        MacAddress = portOneUpdatedMac
                    },
                    new AssetNetworkPortDto
                    {
                        Id = portTwoId,
                        MacAddress = portTwoUpdatedMac
                    }
                }
            };

            using (var scope = _provider.CreateScope())
            {
                var networkService = scope.ServiceProvider.GetRequiredService<INetworkService>();
                var connectionId = await networkService.CreateConnectionAsync(connectionDto);

                var assetService = scope.ServiceProvider.GetRequiredService<IAssetService>();
                var asset = await assetService.GetAssetAsync(_asset.Id);

                var connection = await networkService.GetConnectionAsync(connectionId);

                connection.Ports.Should()
                    .HaveCount(2);
                connection.Ports.Should()
                    .Contain(port => port.Id == portOneId && port.MacAddress == portOneUpdatedMac);
                connection.Ports.Should()
                    .Contain(port => port.Id == portTwoId && port.MacAddress == portTwoUpdatedMac);
            }
        }

        private async Task SetupAssetData(IServiceScope scope)
        {
            var assetService = scope.ServiceProvider.GetRequiredService<IAssetService>();
            await assetService.CreateAssetAsync(_asset);
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
                EthernetPorts = 2,
                PowerPorts = 2
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
                ModelNetworkPort = modelNetworkPort1,
                ModelNetworkPortId = modelNetworkPort1.Id
            };
            var assetNetworkPort2 = new AssetNetworkPortDto()
            {
                Id = Guid.NewGuid(),
                ModelNetworkPort = modelNetworkPort2,
                ModelNetworkPortId = modelNetworkPort2.Id
            };

            var networkPorts = new List<AssetNetworkPortDto>()
            {
                assetNetworkPort1,
                assetNetworkPort2
            };

            var powerPorts = new List<AssetPowerPortDto>()
            {
                new AssetPowerPortDto()
                {
                    Id = Guid.NewGuid()
                },
                new AssetPowerPortDto()
                {
                    Id = Guid.NewGuid()
                }
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
