using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Resources;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class InstancesController : ControllerBase
    {
        private readonly IInstanceService _instanceService;
        private readonly IApiErrorResources _errorResources;

        public InstancesController(IInstanceService instanceService, IApiErrorResources errorResources)
        {
            _instanceService = instanceService;
            _errorResources = errorResources;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<InstanceDto>>> Get(string searchText, int page = 1, int pageSize = 10)
        {
            var instances = await _instanceService.GetInstancesAsync(searchText, page, pageSize);
            return Ok(instances);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InstanceDto>> Get(Guid id)
        {
            var instance = await _instanceService.GetInstanceAsync(id);
            return Ok(instance);
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] InstanceDto instanceDto)
        {
            if (!instanceDto.Id.Equals(default))
            {
                return BadRequest(_errorResources.CannotSetId());
            }

            var id = await _instanceService.CreateInstanceAsync(instanceDto);
            instanceDto.Id = id;

            return CreatedAtAction(nameof(Get), new { id }, instanceDto);
        }

    }
}
