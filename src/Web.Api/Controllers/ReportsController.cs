using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ReportsController : ControllerBase
    {
        private readonly IRackService _rackService;

        public ReportsController(IRackService rackService)
        {
            _rackService = rackService;
        }

        [HttpGet]
        public async Task<ActionResult<List<RackDto>>> Get(Guid? datacenterId)
        {
            const int page = 1;
            const int pageSize = 9999;
            var racks = await _rackService.GetRacksAsync(datacenterId, page, pageSize); 
            return Ok(racks);
        }
    }
}
