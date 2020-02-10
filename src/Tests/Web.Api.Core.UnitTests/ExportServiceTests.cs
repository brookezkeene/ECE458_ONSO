using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Web.Api.Core.Dtos;
using Web.Api.Core.Resources;
using Web.Api.Core.Services;
using Web.Api.Core.UnitTests.Mappers;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories;
using Xunit;
using Xunit.Sdk;

namespace Web.Api.Core.UnitTests
{
    public class ExportServiceTests
    {
        [Fact]
        public async void GetExportModels_FromDatabaseWithTwoModels()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("testdb")
                .Options;

            await using var context = new ApplicationDbContext(options);
            var allModels = GenerateModels();
            await context.Models.AddRangeAsync(allModels);
            var numAdded = await context.SaveChangesAsync();

            var repo = new ModelRepository(context);
            var res = new ModelServiceResources();
            var sut = new ModelService(repo, res);

            // Act
            var query = new ModelExportQuery
            {
                Search = ""
            };
            var result = await sut.GetModelExportAsync(query);
            System.Diagnostics.Debug.WriteLine(result.Count);
            // Assert
            Assert.Equal(result[0].cpu, "storage0");
            Assert.Equal(result.Count, 4);
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
                    Instances = new List<Instance>(),
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
                Instances = new List<Instance>(),
            };
        }
    }
}
