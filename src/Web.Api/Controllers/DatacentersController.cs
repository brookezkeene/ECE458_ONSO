using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common;
using Web.Api.Dtos;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DatacentersController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedList<GetDatacenterApiDto>>> GetMany(string searchText, int page = 1, int pageSize = 10)
        {
            return Ok(null);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDatacenterApiDto>> Get(Guid id)
        {
            return Ok(null);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateDatacenterApiDto datacenter)
        {
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateDatacenterApiDto datacenter)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok();
        }
    }
}