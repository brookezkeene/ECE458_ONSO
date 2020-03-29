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
    public class PowerServiceTests : IClassFixture<BaseMapperFixture>
    {
        private readonly AssetDto _asset;
        private readonly IServiceProvider _provider;

        public PowerServiceTests(BaseMapperFixture fixture)
        {
            _asset = AssetDto();
            _provider = fixture.BuildServiceProvider(services =>
            {
                services.AddSingleton(new Mock<IAuditEventLogger>().Object);
            });
        }

        [Fact]
        public async Task CreateConnection_Success()
        {
            using (var scope = _provider.CreateScope())
            {
                await SetupAssetData(scope);
            }

            var assetPowerPortId = _asset.PowerPorts[0]
                .Id;
            var pduPortId = _asset.Rack.Pdus.First()
                .Ports.First()
                .Id;

            var connectionDto = new PowerConnectionDto
            {
                Ports = new List<PowerPortDto>
                {
                    new AssetPowerPortDto(assetPowerPortId),
                    new PduPortDto(pduPortId)
                }
            };

            using (var scope = _provider.CreateScope())
            {
                var powerService = scope.ServiceProvider.GetRequiredService<IPowerService>();

                var connectionId = await powerService.CreateConnectionAsync(connectionDto);
                var connection = await powerService.GetConnectionAsync(connectionId);

                connection.Ports.Should()
                    .HaveCount(2);
                connection.Ports.Should()
                    .Contain(port => port.Id == pduPortId);
                connection.Ports.Should()
                    .Contain(port => port.Id == assetPowerPortId);
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
                    RackId = rack.Id,
                    Ports = new List<PduPortDto>
                    {
                        new PduPortDto()
                        {
                            Id = Guid.NewGuid(),
                            Number = 1,
                        }
        }
                },
                new PduDto()
                {
                    Id = Guid.NewGuid(),
                    Location = PduLocation.R,
                    Rack = rack,
                    RackId = rack.Id,
                    Ports = new List<PduPortDto>
                    {
                        new PduPortDto()
                        {
                            Id = Guid.NewGuid(),
                            Number = 1
                        }
        }
                }
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
                    Id = Guid.NewGuid(),
                    Number = 1
                },
                new AssetPowerPortDto()
                {
                    Id = Guid.NewGuid(),
                    Number = 2
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

    }
}
