using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ModelsController(IModelService modelService, IApiErrorResources errorResources, IMapper mapper)
        {
            _modelService = modelService;
            _errorResources = errorResources;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<GetModelsApiDto>>> Get([FromQuery] SearchModelQuery query)
        {
            var models = await _modelService.GetModelsAsync(query);
            var response = _mapper.Map<PagedList<GetModelsApiDto>>(models);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetModelApiDto>> Get(Guid id)
        {
            var model = await _modelService.GetModelAsync(id);
            var response = _mapper.Map<GetModelApiDto>(model);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateModelApiDto modelApiDto)
        {
            var modelDto = _mapper.Map<ModelDto>(modelApiDto);
            await _modelService.UpdateModelAsync(modelDto);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateModelApiDto modelApiDto)
        {
            var modelDto = _mapper.Map<ModelDto>(modelApiDto);
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