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
    public class AssetExtensionTests
    {
        [Fact]
        public void SlotsOccupiedTest()
        {
            var assets = GetAssets();
            var expected = assets.Select(ExpectedSlotsOccupied);

            IEnumerable<int> ExpectedSlotsOccupied(Asset asset)
            {
                var start = asset.RackPosition;
                for (var i = 0; i < asset.Model.Height; i++)
                {
                    yield return start + i;
                }
            }

            var actual = assets.Select(asset => asset.SlotsOccupied());

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ConflictsWith_NoConflict_ReturnsFalse()
        {
            var asset = new Asset() {RackPosition = 5, Model = new Model() {Height = 4}}; // spans [U5, U6, U7, U8]
            var other = new Asset() {RackPosition = 1, Model = new Model {Height = 4}}; // spans [U1, U2, U3, U4]

            Assert.False(asset.ConflictsWith(other));
            Assert.False(other.ConflictsWith(asset));
        }

        [Fact]
        public void ConflictsWith_Conflict_ReturnsTrue()
        {
            var asset = new Asset() { RackPosition = 5, Model = new Model() { Height = 4 } }; // spans [U5, U6, U7, U8]
            var other = new Asset() { RackPosition = 2, Model = new Model { Height = 4 } }; // spans [U2, U3, U4, U5]

            Assert.True(asset.ConflictsWith(other));
            Assert.True(other.ConflictsWith(asset));
        }

        private static List<Asset> GetAssets(int count = 20)
        {
            var fakeModel = new Faker<Model>()
                .RuleFor(model => model.Height, faker => faker.Random.Number(1, 42));
            var fakeAsset = new Faker<Asset>()
                .RuleFor(asset => asset.Model, fakeModel.Generate())
                .RuleFor(asset => asset.RackPosition, (faker, asset) => faker.Random.Number(1, 43 - asset.Model.Height));
            return fakeAsset.Generate(count);
        }

    }
}
