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

namespace Web.Api.Core.UnitTests
{
    public class InstanceCRUDTests
    {
        Guid myID = Guid.NewGuid();

        [Fact]
        public async void AddingInstance()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase("testdb")
               .Options;
            await using var context = new ApplicationDbContext(options);

            var allRacks = GenerateRacks("A1", "A2", "A3", "A4", "A5", "B1", "B2", "B3", "C12");
            await context.Racks.AddRangeAsync(allRacks);
            var numAdded = await context.SaveChangesAsync();

            //creating and saving instances into the database 
            var allInstances = GenerateInstances();
            await context.Instances.AddRangeAsync(allInstances);
            await context.SaveChangesAsync();

            var repo = new InstanceRepository(context);
            /*var sut = new InstanceService(repo);

            var instance = new Instance
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
                Hostname = "hostname",
                Rack = new Rack
                {
                    Id = Guid.NewGuid(),
                    Row = "B",
                    Column = 3
                },
                RackPosition = 12
            };
            var instanceDto = instance.ToDto();
            
            var id = instanceDto.Id;
            var result = await sut.CreateInstanceAsync(instanceDto);

            // Assert that the result and the id are the same
            Assert.Equal(result, id);

            //Assert that the created instance exists within the database 
            var getInstance = await sut.GetInstanceAsync(id);
            Assert.Equal(getInstance.Hostname, "hostname");

            System.Diagnostics.Debug.WriteLine("deleting the instance");*/

            /*await sut.DeleteInstanceAsync(id);
            //getInstance = await sut.GetInstanceAsync(id);

            System.Diagnostics.Debug.WriteLine("deleting the instance");

            System.Diagnostics.Debug.WriteLine(getInstance.Id);

            Assert.Equal(getInstance, null);*/


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
