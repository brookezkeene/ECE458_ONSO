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
using Web.Api.Core.Mappers;
using Xunit;
using Xunit.Sdk;
using Web.Api.Dtos;
using Web.Api.Infrastructure.Repositories.Interfaces;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Controllers;
using Web.Api.Resources;

namespace Web.Api.Core.UnitTests
{
    public class DatacenterCrudTests
    {
        [Fact]
        public async void PostModel()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            await using var context = new ApplicationDbContext(options);
            IDatacenterRepository _repository = new DatacenterRepository(context);
            IDatacenterService _service = new DatacenterService(_repository);
            IApiErrorResources _error = new ApiErrorResources();
            DatacentersController _controller = new DatacentersController(_service, _error);

            //checking to see if post works 
            var createDatacenterApiDto = GenerateCreateDatacenterApiDto();
            var sign = await _controller.Post(createDatacenterApiDto);
            Assert.NotNull(sign);

            //checking to see if get works 
            Guid id = Guid.NewGuid();
            var datacenter = GenerateDatacenter(id);
            await context.Datacenters.AddAsync(datacenter);
            var numAdded = await context.SaveChangesAsync();
            var getDatacenter = await _controller.Get(datacenter.Id);
            Assert.NotNull(getDatacenter);



        }
        private static CreateDatacenterApiDto GenerateCreateDatacenterApiDto()
        {
            // confirm proper address format
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
        private static UpdateDatacenterApiDto GenerateUpdateDatacenter(Datacenter datacenter)
        {
            return new UpdateDatacenterApiDto
            {
                Id = datacenter.Id,
                Name = "Research Triangle Park Lab 1",
                Description = "RTP1",
                HasNetworkManagedPower = true
            };
        }
    }
}
