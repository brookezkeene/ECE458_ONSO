using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentValidation.TestHelper;
using Moq;
using Web.Api.Core.Validation;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;
using Xunit;

namespace Web.Api.Core.UnitTests.Validation
{
    public class AssetValidatorTests
    {
        [Fact]
        public async void AssetValidator_FailsIfAssetDoesNotFitInRack()
        {
            var asset = GetAssetThatDoesNotFitInRack();
            var sut = GetDefaultValidator();

            var result = await sut.ValidateAsync(asset);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorCode == "DoesNotFit");
        }


        [Fact]
        public async void AssetValidator_FailsIfAssetHasConflict()
        {
            var asset = GetAssetWithConflict();
            var sut = GetDefaultValidator();

            var result = await sut.ValidateAsync(asset);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.ErrorCode == "Conflict");
        }

        [Fact]
        public async void AssetValidator_PassesIfValidAsset()
        {
            var asset = GetValidAsset();
            var sut = GetAllGreenValidator();

            var result = await sut.ValidateAsync(asset);

            Assert.True(result.IsValid);
        }

        private static AssetValidator GetAllGreenValidator()
        {
            var mockAssetRepo = new Mock<IAssetRepository>();
            mockAssetRepo.Setup(o => o.AssetIsUniqueAsync(It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var mockRackRepo = new Mock<IRackRepository>();
            mockRackRepo.Setup(o => o.AddressExistsAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var mockModelRepo = new Mock<IModelRepository>();
            mockModelRepo.Setup(o => o.ModelExistsAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var mockIdentityRepo = new Mock<IIdentityRepository>();
            mockIdentityRepo.Setup(o => o.GetUserAsync(It.IsAny<Guid>()))
                .ReturnsAsync(new User()); // not null

            return new AssetValidator(mockAssetRepo.Object, mockRackRepo.Object, mockModelRepo.Object,
                mockIdentityRepo.Object);
        }

        private static AssetValidator GetDefaultValidator()
        {
            var mockAssetRepo = new Mock<IAssetRepository>();
            var mockRackRepo = new Mock<IRackRepository>();
            var mockModelRepo = new Mock<IModelRepository>();
            var mockIdentityRepo = new Mock<IIdentityRepository>();

            return new AssetValidator(mockAssetRepo.Object, mockRackRepo.Object, mockModelRepo.Object,
                mockIdentityRepo.Object);
        }

        private static Asset GetAssetThatDoesNotFitInRack()
        {
            var datacenter = new Datacenter { Id = Guid.NewGuid() };
            var rack = new Rack { Column = 1, Row = "A", Datacenter = datacenter };
            var model = new Model {Height = 4};
            var asset = new Asset() {RackPosition = 40, Rack = rack, Model = model};

            rack.Assets = new List<Asset> {asset};
            model.Assets = new List<Asset> {asset};

            return asset;
        }

        private static Asset GetAssetWithConflict()
        {
            var datacenter = new Datacenter { Id = Guid.NewGuid() };
            var rack = new Rack { Column = 1, Row = "A", Datacenter = datacenter };
            var model = new Model { Height = 4 };
            var asset = new Asset() { RackPosition = 5, Rack = rack, Model = model };
            var conflict = new Asset() { RackPosition = 3, Rack = rack, Model = model };

            rack.Assets = new List<Asset> { asset, conflict };
            model.Assets = new List<Asset> { asset, conflict };

            return asset;
        }

        private static Asset GetValidAsset()
        {
            var datacenter = new Datacenter { Id = Guid.NewGuid() };
            var rack = new Rack { Column = 1, Row = "A", Datacenter = datacenter };
            var model = new Model() {Height = 4};
            var asset = new Asset() {RackPosition = 5, Hostname = "server9", Rack = rack, Model = model};
            var otherNoConflict = new Asset() {RackPosition = 9, Hostname = "server8", Rack = rack, Model = model};

            rack.Assets = new List<Asset> {asset, otherNoConflict};
            model.Assets = new List<Asset> { asset, otherNoConflict };

            return asset;
        }
        
    }
}
