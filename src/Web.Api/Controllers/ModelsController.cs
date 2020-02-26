using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web.Api.Common;
using Web.Api.Core;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos;
using Web.Api.Dtos.Models;
using Web.Api.Dtos.Models.Create;
using Web.Api.Dtos.Models.Read;
using Web.Api.Dtos.Models.Update;
using Web.Api.Mappers;
using Web.Api.Resources;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ModelsController : ControllerBase
    {
        private readonly IModelService _modelService;
        private readonly IApiErrorResources _errorResources;

        public ModelsController(IModelService modelService, IApiErrorResources errorResources)
        {
            _modelService = modelService;
            _errorResources = errorResources;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<GetModelsApiDto>>> Get(string searchText, int page = 1, int pageSize = 10)
        {
            var models = await _modelService.GetModelsAsync(searchText, page, pageSize);
            var response = models.MapTo<PagedList<GetModelsApiDto>>();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetModelApiDto>> Get(Guid id)
        {
            var model = await _modelService.GetModelAsync(id);
            var response = model.MapTo<GetModelApiDto>();
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateModelApiDto modelApiDto)
        {
            var modelDto = modelApiDto.MapTo<ModelDto>();
            await _modelService.UpdateModelAsync(modelDto);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateModelApiDto modelApiDto)
        {
            var modelDto = modelApiDto.MapTo<ModelDto>();
            await _modelService.CreateModelAsync(modelDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var model = await _modelService.GetModelAsync(id);
            await _modelService.DeleteModelAsync(model);
            return Ok();
        }
    }
}