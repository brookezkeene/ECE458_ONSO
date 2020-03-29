using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using Skoruba.AuditLogging.Services;
using Web.Api.Configuration.Test;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services;
using Web.Api.Dtos.Bulk.Export;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories;
using Web.Api.IntegrationTests.Tests.Base;
using Web.Api.Mappers;
using Xunit;

namespace Web.Api.IntegrationTests.Tests
{
    public class ExportServiceTests : BaseClassFixture
    {
        protected readonly Mock<IAuditEventLogger> AuditMock;
        private readonly IMapper _mapper;

        public ExportServiceTests(WebApplicationFactory<StartupTest> factory) : base(factory) 
        {
            AuditMock = new Mock<IAuditEventLogger>();
            _mapper = factory.Services.GetService(typeof(IMapper)) as IMapper;
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

            var repo = new ModelRepository<ApplicationDbContext>(context);
            var sut = new ModelService(repo, AuditMock.Object, _mapper);

            // Act
            var query = new ModelExportQuery
            {
                Search = "vendor"
            };
            var models = await sut.GetModelExportAsync(query);
            var result = _mapper.Map<List<ExportModelDto>>(models);
            // Assert
            Assert.Equal("storage0", result[0].cpu);
            Assert.Equal(4, result.Count);
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
