using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos.ChangePlans;
using Web.Api.Mappers;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ChangePlansController : ControllerBase
    {
        private readonly IChangePlanService _changePlanService;

        public ChangePlansController(IChangePlanService changePlanService)
        {
            _changePlanService = changePlanService;
        }
        [HttpGet("{id}/changeplan")]
        public async Task<ActionResult<ChangePlanDto>> GetChangePlan(Guid id)
        {
            var response = await _changePlanService.GetChangePlanAsync(id);
            return Ok(response);
        }
        [HttpGet("{id}/changeplanitem")]
        public async Task<ActionResult<ChangePlanItemDto>> GetChangePlanItem(Guid id)
        {
            var response = await _changePlanService.GetChangePlanItemAsync(id);
            return Ok(response);
        }
        [HttpGet("{id}/changeplans")]
        public async Task<ActionResult<List<ChangePlanDto>>> GetChangePlans(Guid id)
        {
            var response = await _changePlanService.GetChangePlansAsync(id);
            return Ok(response);
        }
        [HttpGet("{id}/changeplanitems")]
        public async Task<ActionResult<List<ChangePlanItemDto>>> GetChangePlanItems(Guid id)
        {
            var response = await _changePlanService.GetChangePlanItemsAsync(id);
            return Ok(response);
        }
        [HttpPost("changeplan")]
        public async Task<IActionResult> Post([FromBody] CreateChangePlanApiDto changePlan)
        {
            var changePlanDto = changePlan.MapTo<ChangePlanDto>();
            changePlanDto.CreatedDate = DateTime.Now;
            await _changePlanService.CreateChangePlanAsync(changePlanDto);
            return Ok();
        }
        [HttpPost("changeplanitem")]
        public async Task<IActionResult> Post([FromBody] CreateChangePlanItemApiDto changePlanItem)
        {
            var changePlanItemDto = changePlanItem.MapTo<ChangePlanItemDto>();
            changePlanItemDto.CreatedDate = DateTime.Now;
            await _changePlanService.CreateChangePlanItemAsync(changePlanItemDto);
            return Ok();
        }
        //TODO: NEED TO SEE IF WE NEED ANOTHER FORMAT/DESERIALIZE DATA OR SOMETHING
        [HttpPut("changeplanitem")]
        public async Task<IActionResult> Put(UpdateChangePlanItemApiDto changePlanItem)
        {
            var changePlanItemDto = changePlanItem.MapTo<ChangePlanItemDto>();
            await _changePlanService.UpdateChangePlanItemAsync(changePlanItemDto);
            return NoContent();
        }
        [HttpDelete("{id}/changeplan")]
        public async Task<IActionResult> DeleteChangePlan(Guid id)
        {
            var changePlan = await _changePlanService.GetChangePlanAsync(id);
            await _changePlanService.DeleteChangePlanAsync(changePlan);
            return Ok();
        }
        [HttpDelete("{id}/changeplanitem")]
        public async Task<IActionResult> DeleteChangePlanItem(Guid id)
        {
            var changePlanItem = await _changePlanService.GetChangePlanItemAsync(id);
            await _changePlanService.DeleteChangePlanItemAsync(changePlanItem);
            return Ok();
        }

    }
}