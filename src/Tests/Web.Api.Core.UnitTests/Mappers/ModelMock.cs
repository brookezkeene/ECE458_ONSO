using System;
using System.Collections.Generic;
using System.Linq;
using Bogus;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.UnitTests.Mappers
{
    public static class UserMock
    {
        internal static Faker<User> GetUserFaker()
        {
            var fakerUser = new Faker<User>()
                .RuleFor(o => o.Id, f => Guid.NewGuid().ToString())
                .RuleFor(o => o.FirstName, f => f.Person.FirstName)
                .RuleFor(o => o.LastName, f => f.Person.LastName)
                .RuleFor(o => o.Email, f => f.Person.Email)
                .RuleFor(o => o.UserName, f => f.Person.UserName);

            return fakerUser;
        }
    }
    public static class InstanceMock
    {
        internal static Faker<Instance> GetInstanceFaker()
        {
            var fakerInstance = new Faker<Instance>()
                .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.Hostname, f => f.Internet.DomainName())
                .RuleFor(o => o.RackPosition, f => f.Random.Number(1, 41))
                .RuleFor(o => o.Comment, f => f.Rant.Review(f.Commerce.Product()));

            return fakerInstance;
        }

        internal static Faker<Instance> AddGeneratedRack(this Faker<Instance> fakerInstance)
        {
            return fakerInstance.RuleFor(o => o.Rack, f => RackMock.GetRackFaker().Generate());
        }

        internal static Faker<Instance> AddGeneratedModel(this Faker<Instance> fakerInstance)
        {
            return fakerInstance.RuleFor(o => o.Model, f => ModelMock.GetModelFaker().Generate());
        }

        public static Instance GenerateRandomInstance()
        {
            var instance = GetInstanceFaker()
                .Generate();

            return instance;
        }

        public static Instance GenerateRandomInstance(Rack rack, Model model)
        {
            var instance = GetInstanceFaker()
                .Generate();
            instance.Rack = rack;
            instance.Model = model;

            return instance;
        }
    }

    public static class RackMock
    {
        internal static Faker<Rack> GetRackFaker()
        {
            var fakerRack = new Faker<Rack>()
                .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.Row, f => f.Random.Char('A', 'B').ToString())
                .RuleFor(o => o.Column, f => f.Random.Number(1, 10));

            return fakerRack;
        }

        public static Rack GenerateRandomRack()
        {
            return GetRackFaker().Generate();
        }
    }

    public static class ModelMock
    {
        internal static Faker<Model> GetModelFaker()
        {
            var fakerModel = new Faker<Model>()
                .RuleFor(o => o.Id, f => Guid.NewGuid())
                .RuleFor(o => o.Vendor, f => f.Random.Word())
                .RuleFor(o => o.ModelNumber, f => f.Random.Word())
                .RuleFor(o => o.Height, f => f.Random.Number(1, 2))
                .RuleFor(o => o.DisplayColor, f => "#" + f.Random.Hash(6))
                .RuleFor(o => o.EthernetPorts, f => f.Random.Number(1, 2))
                .RuleFor(o => o.PowerPorts, f => f.Random.Number(1, 2))
                .RuleFor(o => o.Cpu, f => f.Random.Words(2))
                .RuleFor(o => o.Memory, f => f.Random.Number(1, 64))
                .RuleFor(o => o.Comment, f => f.Rant.Review(f.Commerce.ProductName()));

            return fakerModel;
        }

        internal static Faker<Model> AddGeneratedInstances(this Faker<Model> fakerModel)
        {
            return fakerModel.RuleFor(o => o.Instances, f => InstanceMock.GetInstanceFaker().Generate(f.Random.Number(15)));
        }

        public static Model GenerateRandomModel()
        {
            return GetModelFaker()
                .Generate();
        }

        public static List<Model> GenerateRandomModels(int count = 1)
        {
            return GetModelFaker().Generate(count);
        }
    }
}