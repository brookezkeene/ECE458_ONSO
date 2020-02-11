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

        [HttpGet]//("{id}")] - for datacenter implementation
        public async Task<ActionResult<List<RackDto>>> Get()//string datacenter = "all")
        {
            string searchText = "";
            int page = 1;
            int pageSize = 9999;
            var racks = await _rackService.GetRacksAsync(searchText, page, pageSize); //TODO: will need to filter by datacenter
            return Ok(racks);
        }
    }
}
