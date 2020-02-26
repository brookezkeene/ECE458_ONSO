using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Moq;
using Skoruba.AuditLogging.Services;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services;
using Web.Api.Core.UnitTests.Mappers;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories;
using Web.Api.Core.Mappers;
using Xunit;
using Xunit.Sdk;
using Web.Api.Dtos;
using Web.Api.Infrastructure.Repositories.Interfaces;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Controllers;
using Web.Api.Dtos.Datacenters;
using Web.Api.Dtos.Datacenters.Create;
using Web.Api.Resources;

namespace Web.Api.Core.UnitTests
{
    public class DatacenterCrudTests
    {
        protected readonly Mock<IAuditEventLogger> AuditMock;

        public DatacenterCrudTests()
        {
            AuditMock = new Mock<IAuditEventLogger>();
        }

        [Fact]
        public async void PostDatacenter()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            await using var context = new ApplicationDbContext(options);
            IDatacenterRepository repository = new DatacenterRepository(context);
            IDatacenterService service = new DatacenterService(repository, AuditMock.Object);
            IApiErrorResources error = new ApiErrorResources();
            var controller = new DatacentersController(service, error);

            //checking to see if post works 
            var createDatacenterApiDto = GenerateCreateDatacenterApiDto();
            var sign = await controller.Post(createDatacenterApiDto);
            var result = await context.Datacenters.FirstOrDefaultAsync();
            // weak assertion. TODO: assert property-for-property equality
            Assert.NotNull(result);

            //checking to see if get works 
            var id = Guid.NewGuid();
            var datacenter = GenerateDatacenter(id);
            await context.Datacenters.AddAsync(datacenter);
            var numAdded = await context.SaveChangesAsync();
            var getDatacenter = await controller.Get(datacenter.Id);
            // weak assertion. TODO: assert property-for-property equality
            Assert.NotNull(getDatacenter.Result);

        }
        private static CreateDatacenterApiDto GenerateCreateDatacenterApiDto()
        {
            return new CreateDatacenterApiDto
            {
                Name = "Research Triangle Park Lab 1",
                Description = "RTP1",
                HasNetworkManagedPower = true,
            };
        }
        private static Datacenter GenerateDatacenter(Guid id)
        {
            return new Datacenter
            {
                Id = id,
                Name = "Research Triangle Park Lab 1",
                Description = "RTP1",
                HasNetworkManagedPower = true,
                Racks = new List<Rack>()
            };
        }
    }
}
