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
    public class ModelCrudTests
    {
        [Fact]
        public async void PostModel() // TODO: Why does this test have no asserts?
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
               .UseInMemoryDatabase(Guid.NewGuid().ToString())
               .Options;
            await using var context = new ApplicationDbContext(options);
            IModelRepository _repository = new ModelRepository(context);
            IModelService _service = new ModelService(_repository);
            IApiErrorResources _error = new ApiErrorResources();
            ModelsController _controller = new ModelsController(_service, _error);

            //checking to see if post works 
            var createModelApiDto = GenerateCreateModelApiDto();
            var sign = await _controller.Post(createModelApiDto);
            Assert.NotNull(sign);
            
            //checking to see if get works 
            Guid id = Guid.NewGuid();
            var model = GenerateModel(id);
            await context.Models.AddAsync(model);
            var numAdded = await context.SaveChangesAsync();
            var getModel = await _controller.Get(model.Id);
            Assert.NotNull(getModel);

            //checking to see if update works
            var updateModel = GenerateUpdateModel(model);
            var updated = await _controller.Put(updateModel);
            Assert.NotNull(updated);
        }
        private static CreateModelApiDto GenerateCreateModelApiDto()
        {
            List<CreateModelNetworkPortDto> networkPorts = new List<CreateModelNetworkPortDto>();
            for(int i = 0; i < 4; i++)
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
            
            List<ModelNetworkPort> networkPorts = new List<ModelNetworkPort>();
            for (int i = 0; i < 4; i++)
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
        private static UpdateModelApiDto GenerateUpdateModel(Model model)
        {

            List<UpdateModelNetworkPortDto> networkPorts = new List<UpdateModelNetworkPortDto>();
            for (int i = 0; i < 4; i++)
            {
                networkPorts.Add(new UpdateModelNetworkPortDto { Id = model.NetworkPorts[i].Id, Name = "lol" + (i + 1).ToString(), ModelId = model.Id });
            }
            // confirm proper address format
            return new UpdateModelApiDto
            {
                Id = model.Id,
                Vendor = "vendor",
                ModelNumber = "modelNumber",
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
