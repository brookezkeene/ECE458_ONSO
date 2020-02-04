using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Bogus;
using FluentAssertions;
using Newtonsoft.Json;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Infrastructure.Entities;
using Xunit;

namespace Web.Api.Core.UnitTests.Mappers
{
    public class DomainMapperTests : IDisposable
    {
        private readonly Instance instance;
        private readonly Model model;
        private readonly Rack rack;

        public DomainMapperTests()
        {
            instance = InstanceMock.GenerateRandomInstance();
            model = instance.Model;
            rack = instance.Rack;
        }

        public void Dispose()
        {
            // nothing to dispose of
        }

        [Fact]
        public void CanMapModelsBothWays()
        {
            var modelDto = model.ToDto();
            var mappedModel = modelDto.ToEntity();

            model.Should().BeEquivalentTo(modelDto);
            mappedModel.Should().BeEquivalentTo(modelDto);
            mappedModel.Should().BeEquivalentTo(model);
        }

        [Fact]
        public void CanMapRacksBothWays()
        {
            var rackDto = rack.ToDto();
            var mappedRack = rackDto.ToEntity();

            rack.Should().BeEquivalentTo(rackDto, options => options.ExcludingMissingMembers());
            mappedRack.Should().BeEquivalentTo(rackDto, options => options.ExcludingMissingMembers());
            mappedRack.Should().BeEquivalentTo(rack);
        }

        [Fact]
        public void CanMapInstancesBothWays()
        {
            var instanceDto = instance.ToDto();
            var mappedDto = instanceDto.ToEntity();

            instance.Should().BeEquivalentTo(instanceDto, options => options.ExcludingNestedObjects());
            mappedDto.Should().BeEquivalentTo(instanceDto, options => options.ExcludingNestedObjects());
            mappedDto.Should().BeEquivalentTo(instance, options => options.ExcludingNestedObjects());
        }

        //[Fact]
        //public void PrintJson()
        //{
        //    var racks = RackMock.GetRackFaker()
        //        .Generate(5);
        //    var models = ModelMock.GetModelFaker()
        //        .Generate(15);
        //    var instances = InstanceMock.GetInstanceFaker()
        //        .Generate(50);

        //    var faker = new Faker();
        //    foreach (var instance in instances)
        //    {
        //        instance.Model = faker.PickRandom(models);
        //        instance.Rack = faker.PickRandom(racks);
        //    }

        //    foreach (var rack in racks)
        //    {
        //        for (var i = 1; i <= 42; i++)
        //        {
        //            var instancesInSlot = instances.Where(o => o.Rack.Id == rack.Id && o.RackPosition <= i && i < o.RackPosition + o.Model.Height)
        //                    .ToList();
        //            if (instancesInSlot.Count() > 1)
        //            {
        //                var toRemove = instancesInSlot.First();
        //                instances.Remove(toRemove);
        //            }
        //        }
        //    }

        //    var racksJson = JsonConvert.SerializeObject(racks);
        //    var modelsJson = JsonConvert.SerializeObject(models);
        //    var instancesJson = JsonConvert.SerializeObject(instances);

        //    Console.WriteLine("done");
        //}
    }
}
