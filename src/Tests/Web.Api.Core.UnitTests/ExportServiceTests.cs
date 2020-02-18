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
using Xunit;
using Xunit.Sdk;

namespace Web.Api.Core.UnitTests
{
    public class ExportServiceTests
    {
        [Fact]
        public async void GetExportInstances()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            await using var context = new ApplicationDbContext(options);
            
            var allModels = GenerateModels();
            await context.Models.AddRangeAsync(allModels);
            await context.SaveChangesAsync();

            /*var allRacks = GenerateRacks();
            await context.Racks.AddRangeAsync(allRacks);
            await context.SaveChangesAsync();*/

            var allInstances = GenerateInstances();
            await context.Instances.AddRangeAsync(allInstances);
            await context.SaveChangesAsync();

            /*var repo = new InstanceRepository(context);
            var sut = new InstanceService(repo);

            // Act
            var query = new InstanceExportQuery
            {    
                StartRow = "a",
                StartCol = 1,
                *//*EndRow = "b",
                EndCol = 5,*//*
                Hostname = "name2",
                Search = "num2"
            };

            var result = await sut.GetInstanceExportAsync(query);
            System.Diagnostics.Debug.WriteLine("num of instances in result");
            System.Diagnostics.Debug.WriteLine(result.Count);

            // Assert
            Assert.Equal(result.Count, 1);*/
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
            var sut = new ModelService(repo);

            // Act
            var query = new ModelExportQuery
            {
                Search = "vendor"
            };
            var result = await sut.GetModelExportAsync(query);
            System.Diagnostics.Debug.WriteLine(result.Count);
            // Assert
            Assert.Equal(result[0].cpu, "storage0");
            Assert.Equal(result.Count, 4);
        }

        private static IEnumerable<Instance> GenerateInstances()
        {
            for (int i = 0; i < 4; i++)
            {
                // confirm proper address format
                yield return new Instance
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
            yield return new Instance
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
