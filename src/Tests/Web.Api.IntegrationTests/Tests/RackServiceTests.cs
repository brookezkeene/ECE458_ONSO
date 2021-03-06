﻿using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using Skoruba.AuditLogging.Services;
using Web.Api.Configuration.Test;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories;
using Web.Api.IntegrationTests.Tests.Base;
using Xunit;

namespace Web.Api.IntegrationTests.Tests
{
    public class RackServiceTests : BaseClassFixture
    {
        protected readonly Mock<IAuditEventLogger> AuditMock;
        private readonly IMapper _mapper;
        public RackServiceTests(WebApplicationFactory<StartupTest> factory) : base(factory)
        {
            _mapper = factory.Services.GetService(typeof(IMapper)) as IMapper;
            AuditMock = new Mock<IAuditEventLogger>();
        }

        public async void GetRacks_ForRangeWithNoRacks_ReturnsEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            await using var context = new ApplicationDbContext(options);
            var allRacks = GenerateRacks("A1", "A2", "A3", "A4", "A5", "B1", "B2", "B3");
            await context.Racks.AddRangeAsync(allRacks);
            var numAdded = await context.SaveChangesAsync();

            var repo = new RackRepository<ApplicationDbContext>(context);
            var sut = new RackService(repo, AuditMock.Object, _mapper);


            // Act
            var query = new RackRangeQuery
            {
                StartRow = "A",
                StartCol = 1,
                EndRow = "A",
                EndCol = 5
            };
            var result = await sut.GetRacksAsync(query);

            // Assert
            Assert.Equal(5, result.Count);
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
