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
using Web.Api.Dtos.Models;
using Web.Api.Dtos.Models.Create;
using Web.Api.Resources;

namespace Web.Api.Core.UnitTests
{
    public class ModelCrudTests
    {
        protected readonly Mock<IAuditEventLogger> AuditMock;

        public ModelCrudTests()
        {
            AuditMock = new Mock<IAuditEventLogger>();
        }

        [Fact]
        public async void PostModel()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            await using var context = new ApplicationDbContext(options);
            IModelRepository repository = new ModelRepository(context);
            IModelService service = new ModelService(repository, AuditMock.Object);
            IApiErrorResources error = new ApiErrorResources();
            var controller = new ModelsController(service, error);

            //checking to see if post works 
            var createModelApiDto = GenerateCreateModelApiDto();
            var sign = await controller.Post(createModelApiDto);
            var result = await context.Models.FirstOrDefaultAsync();
            // weak assertion. TODO: assert property-for-property equality
            Assert.NotNull(result);




            //checking to see if get works 
            var id = Guid.NewGuid();
            var model = GenerateModel(id);

            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            var json = new JavaScriptSerializer().Serialize(model);

            await context.Models.AddAsync(model);
            var numAdded = await context.SaveChangesAsync();
            var getModel = await controller.Get(model.Id);
            // weak assertion. TODO: assert property-for-property equality
            Assert.NotNull(getModel.Result);
        }

        private static CreateModelApiDto GenerateCreateModelApiDto()
        {
            var networkPorts = new List<CreateModelNetworkPortDto>();
            for (var i = 0; i < 4; i++)
            {
                networkPorts.Add(new CreateModelNetworkPortDto { Name = (i + 1).ToString() });
            }
            // confirm proper address format
            return new CreateModelApiDto
            {

                Vendor = "vendor",
                ModelNumber = "modelNumber",
                Height = 2,
                DisplayColor = "ffffff",
                Cpu = "cpu",
                Storage = "storage",
                Comment = "comment",
                Memory = 10,
                EthernetPorts = 4,
                PowerPorts = 4,
                NetworkPorts = networkPorts,

            };
        }
        private static Model GenerateModel(Guid id)
        {

            var networkPorts = new List<ModelNetworkPort>();
            for (var i = 0; i < 4; i++)
            {
                networkPorts.Add(new ModelNetworkPort { Name = (i + 1).ToString() });
            }
            // confirm proper address format
            return new Model
            {
                Id = id,
                Vendor = "vendor",
                ModelNumber = "modelNumber",
                Height = 2,
                DisplayColor = "ffffff",
                Cpu = "cpu",
                Storage = "storage",
                Comment = "comment",
                Memory = 10,
                EthernetPorts = 4,
                PowerPorts = 4,
                NetworkPorts = networkPorts,

            };
        }
    }
}
