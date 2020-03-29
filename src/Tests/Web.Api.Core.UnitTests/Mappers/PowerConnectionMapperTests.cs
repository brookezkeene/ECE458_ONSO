using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Skoruba.AuditLogging.Services;
using Web.Api.Controllers;
using Web.Api.Core.Dtos;
using Web.Api.Core.UnitTests.Common;
using Web.Api.Infrastructure.DbContexts;
using Xunit;

namespace Web.Api.Core.UnitTests.Mappers
{
    public class PowerConnectionMapperTests : IClassFixture<BaseMapperFixture>
    {
        private readonly IMapper _mapper;

        public PowerConnectionMapperTests(BaseMapperFixture fixture)
        {
            _mapper = fixture.GetMapper(services =>
            {
                services.AddSingleton(new Mock<IAuditEventLogger>().Object);
            });
        }

        [Fact]
        public void CanMapCreatePowerConnectionApiDto_ToPowerConnectionDto()
        {
            var apiDto = new CreatePowerConnectionApiDto
            {
                PduPortId = Guid.NewGuid(),
                AssetPowerPortId = Guid.NewGuid()
            };

            var result = _mapper.Map<PowerConnectionDto>(apiDto);

            result.Ports.Should()
                .HaveCount(2);
            result.Ports.Should()
                .ContainSingle(port => port is AssetPowerPortDto)
                .Which.Id.Should()
                .Be(apiDto.AssetPowerPortId);
            result.Ports.Should()
                .ContainSingle(port => port is PduPortDto)
                .Which.Id.Should()
                .Be(apiDto.PduPortId);
        }
    }
}
