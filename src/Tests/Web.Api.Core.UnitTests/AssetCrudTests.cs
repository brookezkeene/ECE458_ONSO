using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services;
using Web.Api.Core.UnitTests.Mappers;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories;
using Web.Api.Core.Mappers;
using Xunit;
using Xunit.Sdk;
using Web.Api.Dtos;
using Web.Api.Infrastructure.Repositories.Interfaces;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Controllers;
using Web.Api.Resources;

namespace Web.Api.Core.UnitTests
{
    public class AssetCrudTests
    {
        [Fact]
        public async void AddingAsset() // TODO: Why does this test have no asserts?
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            await using var context = new ApplicationDbContext(options);

            var allRacks = GenerateRacks("A1", "A2", "A3", "A4", "A5", "B1", "B2", "B3", "C12");
            await context.Racks.AddRangeAsync(allRacks);
            var numAdded = await context.SaveChangesAsync();

            //creating and saving assets into the database 
            var allAssets = GenerateAssets();
            await context.Assets.AddRangeAsync(allAssets);
            await context.SaveChangesAsync();

            var repo = new AssetRepository(context);
        }

        [Fact]
        public async void AddingCreateModel() // TODO: Why does this test have no asserts?
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            await using var context = new ApplicationDbContext(options);
            IModelRepository _repository = new ModelRepository(context);
            IModelService _service = new ModelService(_repository);
            IApiErrorResources _error = new ApiErrorResources();
            ModelsController _controller = new ModelsController(_service, _error);

            var createModelApiDto = GenerateCreateModelApiDto();
            var exModel = context.Model;
            
            var sign = await _controller.Post(createModelApiDto);
            Assert.NotNull(sign);
        }
        private static CreateModelApiDto GenerateCreateModelApiDto()
        {
            List<CreateModelNetworkPortDto> networkPorts = new List<CreateModelNetworkPortDto>();
            for (int i = 0; i < 4; i++)
            {
                networkPorts.Add(new CreateModelNetworkPortDto { Name = (i + 1).ToString() });
            }
            // confirm proper address format
            return new CreateModelApiDto
            {
                Vendor = "vendor",
                ModelNumber = "modelNumber",
                Height = 2,
                DisplayColor = "ffffff",
                Cpu = "cpu",
                Storage = "storage",
                Comment = "comment",
                Memory = 10,
                EthernetPorts = 4,
                PowerPorts = 4,
                NetworkPorts = networkPorts,

            };

        }

        private static IEnumerable<Asset> GenerateAssets()
        {
            for (int i = 0; i < 4; i++)
            {
                // confirm proper address format
                yield return new Asset
                {
                    Id = Guid.NewGuid(),
                    Model = new Model
                    {
                        Id = Guid.NewGuid(),
                        Vendor = "vendor",
                        ModelNumber = "num" + i.ToString(),
                        Height = 2,
                        DisplayColor = "#ffffff",
                        EthernetPorts = 2,
                        PowerPorts = 2,
                        Cpu = "storage" + i.ToString(),
                        Memory = 43,
                        Storage = "stor",

                    },
                    Hostname = "name" + i.ToString(),
                    Rack = new Rack
                    {
                        Id = Guid.NewGuid(),
                        Row = "A",
                        Column = i + 1
                    },
                    RackPosition = 12
                };
            }
            yield return new Asset
            {
                Id = Guid.NewGuid(),
                Model = new Model
                {
                    Id = Guid.NewGuid(),
                    Vendor = "ve2ndor2",
                    ModelNumber = "num2",
                    Height = 2,
                    DisplayColor = "#ffffff",
                    EthernetPorts = 2,
                    PowerPorts = 2,
                    Cpu = "storage",
                    Memory = 43,
                    Storage = "stor",

                },
                Hostname = "name",
                Rack = new Rack
                {
                    Id = Guid.NewGuid(),
                    Row = "B",
                    Column = 3
                },
                RackPosition = 12
            };

        }
        private static IEnumerable<Rack> GenerateRacks(params string[] addresses)
        {
            foreach (var address in addresses)
            {
                // confirm proper address format

                if (!char.IsLetter(address, 0) || !int.TryParse(address.Substring(1), out var col))
                {
                    throw new ArgumentException($"Attempted to generate invalid rack address, {address}");
                }

                yield return new Rack
                {
                    Id = Guid.NewGuid(),
                    Row = address.Substring(0, 1),
                    Column = col
                };
            }
        }
    }
}
