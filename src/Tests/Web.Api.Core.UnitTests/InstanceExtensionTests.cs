using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bogus;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Entities.Extensions;
using Xunit;

namespace Web.Api.Core.UnitTests
{
    public class InstanceExtensionTests
    {
        [Fact]
        public void SlotsOccupiedTest()
        {
            var instances = GetInstances();
            var expected = instances.Select(ExpectedSlotsOccupied);

            IEnumerable<int> ExpectedSlotsOccupied(Instance instance)
            {
                var start = instance.RackPosition;
                for (var i = 0; i < instance.Model.Height; i++)
                {
                    yield return start + i;
                }
            }

            var actual = instances.Select(instance => instance.SlotsOccupied());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConflictsWith_NoConflict_ReturnsFalse()
        {
            var instance = new Instance() {RackPosition = 5, Model = new Model() {Height = 4}}; // spans [U5, U6, U7, U8]
            var other = new Instance() {RackPosition = 1, Model = new Model {Height = 4}}; // spans [U1, U2, U3, U4]

            Assert.False(instance.ConflictsWith(other));
            Assert.False(other.ConflictsWith(instance));
        }

        [Fact]
        public void ConflictsWith_Conflict_ReturnsTrue()
        {
            var instance = new Instance() { RackPosition = 5, Model = new Model() { Height = 4 } }; // spans [U5, U6, U7, U8]
            var other = new Instance() { RackPosition = 2, Model = new Model { Height = 4 } }; // spans [U2, U3, U4, U5]

            Assert.True(instance.ConflictsWith(other));
            Assert.True(other.ConflictsWith(instance));
        }

        private static List<Instance> GetInstances(int count = 20)
        {
            var fakeModel = new Faker<Model>()
                .RuleFor(model => model.Height, faker => faker.Random.Number(1, 42));
            var fakeInstance = new Faker<Instance>()
                .RuleFor(instance => instance.Model, fakeModel.Generate())
                .RuleFor(instance => instance.RackPosition, (faker, instance) => faker.Random.Number(1, 43 - instance.Model.Height));
            return fakeInstance.Generate(count);
        }

    }
}
