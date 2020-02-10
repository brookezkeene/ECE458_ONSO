using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services;
using Web.Api.Core.UnitTests.Mappers;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories;
using Xunit;
using Xunit.Sdk;

namespace Web.Api.Core.UnitTests
{
    public class RackServiceTests
    {
        [Fact]
        public async void GetRacks_ForRangeWithNoRacks_ReturnsEmpty()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("testdb")
                .Options;

            await using var context = new ApplicationDbContext(options);
            var allRacks = GenerateRacks("A1", "A2", "A3", "A4", "A5", "B1", "B2", "B3");
            await context.Racks.AddRangeAsync(allRacks);
            var numAdded = await context.SaveChangesAsync();
            
            var repo = new RackRepository(context);
            var sut = new RackService(repo);


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
            Assert.Equal(result.Count, 5);

            var query1 = new RackRangeQuery
            {
                StartRow = "A",
                StartCol = 1,
                EndRow = "A",
                EndCol = 3
            };

            await sut.DeleteRacksAsync(query1);
            var resultDelete = await sut.GetRacksAsync(query1);
            var resultGet = await sut.GetRacksAsync(query);

            System.Diagnostics.Debug.WriteLine(resultDelete.Count);
            System.Diagnostics.Debug.WriteLine(resultGet.Count);

            Assert.Equal(resultDelete.Count, 0);
            Assert.Equal(resultGet.Count, 2);


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
