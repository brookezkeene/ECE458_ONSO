using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChangePlanController : ControllerBase
    {
        private readonly IChangePlanService _changePlanService;

        public ChangePlanController(IChangePlanService changePlanService)
        {
            _changePlanService = changePlanService;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChangePlanDto changePlanDto)
        {
            await _changePlanService.CreateChangePlanAsync(changePlanDto);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChangePlanItemDto changePlanItemDto)
        {
            await _changePlanService.CreateChangePlanItemAsync(changePlanItemDto);
            return Ok();
        }
    }
}