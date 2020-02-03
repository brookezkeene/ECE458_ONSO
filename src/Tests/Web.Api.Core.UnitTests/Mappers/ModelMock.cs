using System;
using System.Collections.Generic;
using Bogus;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.UnitTests.Mappers
{
    public static class ModelMock
    {
        public static Faker<Model> GetModelFaker()
        {
            var fakerModel = new Faker<Model>()
                .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.Vendor, f => f.Random.Word())
                .RuleFor(o => o.ModelNumber, f => f.Random.Word())
                .RuleFor(o => o.Height, f => f.Random.Number(1, 4))
                .RuleFor(o => o.DisplayColor, f => "#" + f.Random.Hash(6))
                .RuleFor(o => o.EthernetPorts, f => f.Random.Number(1, 2))
                .RuleFor(o => o.PowerPorts, f => f.Random.Number(1, 2))
                .RuleFor(o => o.Cpu, f => f.Random.Words(2))
                .RuleFor(o => o.Memory, f => f.Random.Number(1, 64))
                .RuleFor(o => o.Comment, f => f.Random.Words(f.Random.Number(6, 10)))
                .RuleFor(o => o.Instances, f => GetInstanceFaker().Generate(f.Random.Number(15)));

            return fakerModel;
        }

        public static Faker<Instance> GetInstanceFaker()
        {
            var fakerInstance = new Faker<Instance>()
                .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.Hostname, f => f.Internet.DomainName())
                .RuleFor(o => o.RackPosition, f => f.Random.Number(1, 39));

            return fakerInstance;
        }

        public static Model GenerateRandomModel()
        {
            return GetModelFaker().Generate();
        }

        public static List<Model> GenerateRandomModels(int count = 1)
        {
            return GetModelFaker().Generate(count);
        }

        public static Instance GenerateRandomInstance()
        {
            return GetInstanceFaker().Generate();
        }

        public static Rack GenerateRandomRack()
        {
            //return GetRackFaker(Guid.NewGuid()).Generate();
            throw new NotImplementedException();
        }
    }
}