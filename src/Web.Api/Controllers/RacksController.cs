using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos;
using Web.Api.Dtos.Racks;
using Web.Api.Mappers;
using Web.Api.Resources;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RacksController : ControllerBase
    {
        private readonly IRackService _rackService;

        public RacksController(IRackService rackService)
        {
            _rackService = rackService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<GetRacksApiDto>>> Get(Guid? datacenterId, int page = 1, int pageSize = 10)
        {
            var racks = await _rackService.GetRacksAsync(datacenterId, page, pageSize);
            var response = racks.MapTo<PagedList<GetRacksApiDto>>();
            return Ok(response);
        }

        [HttpGet("range")]
        public async Task<ActionResult<List<GetRacksApiDto>>> Get([FromQuery] RackRangeQuery query)
        {
            var racks = await _rackService.GetRacksAsync(query);
            var response = racks.MapTo<List<GetRacksApiDto>>();
            return Ok(response);
        }

        [HttpPost("range")]
        public async Task<ActionResult> Post([FromQuery] RackRangeQuery query)
        {
            await _rackService.CreateRacksAsync(query);
            return Ok();
        }

        [HttpDelete("range")]
        public async Task<ActionResult> Delete([FromQuery] RackRangeQuery query)
        {
            await _rackService.DeleteRacksAsync(query);
            return Ok();
        }

        [HttpGet("{id}/pdus")]
        public async Task<ActionResult<List<GetRackPdusApiDto>>> GetRackPdus(Guid id)
        {
            return Ok(null);
        }
    }
}