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
using Web.Api.Resources;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ModelsController : ControllerBase
    {
        private IModelService _modelService;
        private readonly IApiErrorResources _errorResources;

        public ModelsController(IModelService modelService)
        {
            _modelService = modelService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<FlatModelDto>>> Get(string searchText, int page = 1, int pageSize = 10)
        {
            var models = await _modelService.GetModelsAsync(searchText, page, pageSize);
            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ModelDto>> Get(Guid id)
        {
            var model = await _modelService.GetModelAsync(id);

            return Ok(model);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ModelDto modelDto)
        {
            await _modelService.UpdateModelAsync(modelDto);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post(ModelDto modelDto)
        {
            if (!modelDto.Id.Equals(default))
            {
                return BadRequest(_errorResources.CannotSetId());
            }

            var id = await _modelService.CreateModelAsync(modelDto);
            modelDto.Id = id;

            return CreatedAtAction(nameof(Get), new {id = id}, modelDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _modelService.DeleteModelAsync(id);
            return Ok();
        }
    }
}