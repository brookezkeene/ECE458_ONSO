using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class InstancesController : ControllerBase
    {
        private readonly IInstanceService _instanceService;

        public InstancesController(IInstanceService instanceService)
        {
            _instanceService = instanceService;
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
    }
}
