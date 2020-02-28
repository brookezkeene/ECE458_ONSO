using System;
using System.Collections.Generic;
using System.Text;
using Bogus;
using FluentValidation;
using FluentValidation.TestHelper;
using Microsoft.EntityFrameworkCore.Metadata;
using Moq;
using Web.Api.Core.Validation;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;
using Xunit;

namespace Web.Api.Core.UnitTests.Validation
{
    public class ModelValidatorTests
    {
        private const string LongString = "this string is surely more than fifty characters by now, but just in case we will make it even longer";

        [Fact]
        public void ModelValidator_FailsIfVendorIsNull()
        {
            var sut = GetDefaultValidator();
            sut.ShouldHaveValidationErrorFor(x => x.Vendor, null as string);
        }

        [Fact]
        public void ModelValidator_FailsIfVendorTooLong()
        {
            var sut = GetDefaultValidator();
            sut.ShouldHaveValidationErrorFor(x => x.Vendor, LongString);
        }

        [Fact]
        public void ModelValidator_FailsIfModelNumberIsNull()
        {
            var sut = GetDefaultValidator();
            sut.ShouldHaveValidationErrorFor(x => x.ModelNumber, null as string);
        }

        [Fact]
        public void ModelValidator_FailsIfModelNumberTooLong()
        {
            var sut = GetDefaultValidator();
            sut.ShouldHaveValidationErrorFor(x => x.ModelNumber, LongString);
        }

        [Fact]
        public void ModelValidator_FailsIfHeightIsOutOfRange()
        {
            var sut = GetDefaultValidator();
            sut.ShouldHaveValidationErrorFor(x => x.Height, 0);
            sut.ShouldHaveValidationErrorFor(x => x.Height, -1);
            sut.ShouldHaveValidationErrorFor(x => x.Height, 43);
        }

        [Fact]
        public void ModelValidator_FailsIfDisplayColorNotValid()
        {
            var sut = GetDefaultValidator();
            sut.ShouldHaveValidationErrorFor(x => x.DisplayColor, "not a hex color");
        }

        [Fact]
        public void ModelValidator_PassesIfDisplayColorValid()
        {
            var sut = GetDefaultValidator();
            sut.ShouldNotHaveValidationErrorFor(x => x.DisplayColor, "#0f0f0f");
            sut.ShouldNotHaveValidationErrorFor(x => x.DisplayColor, "#FFFFFF");
            sut.ShouldNotHaveValidationErrorFor(x => x.DisplayColor, null as string);
        }

        [Fact]
        public void ModelValidator_FailsIfEthernetPortsNegative()
        {
            var sut = GetDefaultValidator();
            sut.ShouldHaveValidationErrorFor(x => x.EthernetPorts, -1);
        }

        [Fact]
        public void ModelValidator_FailsIfPowerPortsNegative()
        {
            var sut = GetDefaultValidator();
            sut.ShouldHaveValidationErrorFor(x => x.PowerPorts, -1);
        }

        [Fact]
        public void ModelValidator_FailsIfMemoryNegative()
        {
            var sut = GetDefaultValidator();
            sut.ShouldHaveValidationErrorFor(x => x.Memory, -1);
        }

        [Fact]
        public void ModelValidator_FailsIfCpuTooLong()
        {
            var sut = GetDefaultValidator();
            sut.ShouldHaveValidationErrorFor(x => x.Cpu, LongString);
        }

        [Fact]
        public void ModelValidator_FailsIfStorageTooLong()
        {
            var sut = GetDefaultValidator();
            sut.ShouldHaveValidationErrorFor(x => x.Storage, LongString);
        }

        [Fact]
        public async void ModelValidator_Create_FailsIfNotUnique()
        {
            var mockRepo = new Mock<IModelRepository>();
            mockRepo.Setup(x => x.ModelIsUniqueAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Guid>()))
                .ReturnsAsync(false);

            var sut = new ModelValidator(mockRepo.Object);

            var model = GetValidModelWithAssets();

            var result = await sut.ValidateAsync(model);

            Assert.False(result.IsValid);
        }

        [Fact]
        public async void ModelValidator_Update_FailsIfHeightChangeCriteriaNotMet()
        {
            var mockRepo = new Mock<IModelRepository>();
            mockRepo.Setup(x => x.MeetsHeightChangeCriteriaAsync(It.IsAny<Model>()))
                .ReturnsAsync(false);

            var sut = new ModelValidator(mockRepo.Object);

            var model = GetValidModelWithAssets();

            var result = await sut.ValidateAsync(model, ruleSet: "default, update");

            Assert.False(result.IsValid);
        }

        [Fact]
        public async void ModelValidator_Delete_FailsIfAssetsExist()
        {
            var sut = GetDefaultValidator();
            var model = GetValidModelWithAssets();

            var result = await sut.ValidateAsync(model, ruleSet: "delete");

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == nameof(model.Assets));
        }

        private static ModelValidator GetDefaultValidator()
        {
            var mockRepo = new Mock<IModelRepository>();
            return new ModelValidator(mockRepo.Object);
        }

        private static Model GetValidModelWithAssets()
        {
            var model = new Model() { Vendor = "vendor", ModelNumber = "modelno", Height = 4 };
            var assets = new List<Asset>
            {
                new Asset() {Model = model, RackPosition = 1},
                new Asset() {Model = model, RackPosition = 5},
                new Asset() {Model = model, RackPosition = 9},
                new Asset() {Model = model, RackPosition = 13}
            };
            model.Assets = assets;

            return model;
        }
    }
}
