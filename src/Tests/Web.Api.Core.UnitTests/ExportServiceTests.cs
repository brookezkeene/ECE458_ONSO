using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Moq;
using Skoruba.AuditLogging.Services;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services;
using Web.Api.Core.UnitTests.Mappers;
using Web.Api.Dtos;
using Web.Api.Dtos.Bulk.Export;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories;
using Web.Api.Mappers;
using Xunit;
using Xunit.Sdk;

namespace Web.Api.Core.UnitTests
{
    public class ExportServiceTests
    {
        protected readonly Mock<IAuditEventLogger> AuditMock;

        public ExportServiceTests()
        {
            AuditMock = new Mock<IAuditEventLogger>();
        }

        [Fact]
        public async void GetExportAssets() // TODO: Why does this test have no asserts?
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            await using var context = new ApplicationDbContext(options);
            
            var allModels = GenerateModels();
            await context.Models.AddRangeAsync(allModels);
            await context.SaveChangesAsync();

            var allAssets = GenerateAssets();
            await context.Assets.AddRangeAsync(allAssets);
            await context.SaveChangesAsync();
        }

        [Fact]
        public async void GetExportModels_FromDatabaseWithTwoModels()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using var context = new ApplicationDbContext(options);
            var allModels = GenerateModels();
            await context.Models.AddRangeAsync(allModels);
            var numAdded = await context.SaveChangesAsync();

            var repo = new ModelRepository(context);
            var sut = new ModelService(repo, AuditMock.Object);

            // Act
            var query = new ModelExportQuery
            {
                Search = "vendor"
            };
            var result = (await sut.GetModelExportAsync(query)).MapTo<List<ExportModelDto>>();
            // Assert
            Assert.Equal(result[0].cpu, "storage0");
            Assert.Equal(result.Count, 4);
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
                    Hostname = "name"+i.ToString(),
                    Rack = new Rack
                    {
                        Id = Guid.NewGuid(),
                        Row = "A",
                        Column = i+1
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
 
        private static IEnumerable<Model> GenerateModels()
        {
            for (int i = 0; i < 3; i++)
            {
                // confirm proper address format
                yield return new Model
                {
                    Id = Guid.NewGuid(),
                    Vendor = "vendor" + i.ToString(),
                    ModelNumber = "num",
                    Height = 2,
                    DisplayColor = "#ffffff",
                    EthernetPorts = 2,
                    PowerPorts = 2,
                    Cpu = "storage" + i.ToString(),
                    Memory = 43,
                    Storage = "stor",
                    Comment = "",
                    Assets = new List<Asset>(),
                };
            }
            yield return new Model
            {
                Id = Guid.NewGuid(),
                Vendor = "vendor1",
                ModelNumber = "num1",
                Height = 2,
                DisplayColor = "#ffffff",
                EthernetPorts = 2,
                PowerPorts = 2,
                Cpu = "storage",
                Memory = 43,
                Storage = "stor",
                Comment = "",
                Assets = new List<Asset>(),
            };
        }
    }
}
