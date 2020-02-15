using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using FluentValidation;
using FluentValidation.TestHelper;
using Moq;
using Web.Api.Core.Validation;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;
using Xunit;

namespace Web.Api.Core.UnitTests.Validation
{
    public class RackValidatorTests
    {
        [Fact]
        public async void RackValidator_Delete_FailsIfContainsInstances()
        {
            var mockRepo = new Mock<IRackRepository>();
            var sut = new RackValidator(mockRepo.Object);

            var rack = GetValidRackWithInstances();

            var result = await sut.ValidateAsync(rack, ruleSet: "delete");

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors,
                validationFailure => validationFailure.PropertyName == nameof(rack.Instances));
        }

        [Fact]
        public void RackValidator_Delete_PassesIfNoInstances()
        {
            var mockRepo = new Mock<IRackRepository>();
            var sut = new RackValidator(mockRepo.Object);
            
            sut.ShouldNotHaveValidationErrorFor(x => x.Instances, null as List<Instance>);
        }

        [Fact]
        public async void RackValidator_Create_FailsIfAddressAlreadyExists()
        {
            var mockRepo = new Mock<IRackRepository>();
            mockRepo.Setup(x => x.AddressExistsAsync(It.IsAny<string>(), It.IsAny<int>()))
                .ReturnsAsync(true);
            var sut = new RackValidator(mockRepo.Object);

            var rack = GetValidRackWithInstances();

            var result = await sut.ValidateAsync(rack, ruleSet: "create");

            Assert.False(result.IsValid);
        }

        private static Rack GetValidRackWithInstances()
        {
            var rack = GetValidRackWithoutInstances();
            var model = new Model() { Height = 4};
            var instances = new List<Instance>
            {
                new Instance() {Rack = rack, Model = model, RackPosition = 1},
                new Instance() {Rack = rack, Model = model, RackPosition = 5},
                new Instance() {Rack = rack, Model = model, RackPosition = 9},
                new Instance() {Rack = rack, Model = model, RackPosition = 13}
            };
            model.Instances = instances;
            rack.Instances = instances;

            return rack;
        }

        private static Rack GetValidRackWithoutInstances()
        {
            var rack = new Rack() {Row = "A", Column = 1};
            return rack;
        }
    }
}
