using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos.ChangePlans;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ChangePlansController : ControllerBase
    {
        private readonly IChangePlanService _changePlanService;
        private readonly IMapper _mapper;

        public ChangePlansController(IChangePlanService changePlanService, IMapper mapper)
        {
            _changePlanService = changePlanService;
            _mapper = mapper;
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
            var changePlanDto = _mapper.Map<ChangePlanDto>(changePlan);
            await _changePlanService.CreateChangePlanAsync(changePlanDto);
            return Ok();
        }

        [HttpPost("changeplanitem")]
        public async Task<IActionResult> Post([FromBody] CreateChangePlanItemApiDto changePlanItem)
        {
            var changePlanItemDto = _mapper.Map<ChangePlanItemDto>(changePlanItem);
            await _changePlanService.CreateChangePlanItemAsync(changePlanItemDto);
            return Ok();
        }

        [HttpPut("changeplan")]
        public async Task<IActionResult> Put(ChangePlanDto changePlan)
        {
            await _changePlanService.UpdateChangePlanAsync(changePlan);
            return NoContent();
        }

        [HttpPut("changeplanitem")]
        public async Task<IActionResult> Put(ChangePlanItemDto changePlanItem)
        {
            await _changePlanService.UpdateChangePlanItemAsync(changePlanItem);
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

        /**
         * The data inside of the change plan items are entities! 
         * (asset and decommissionedasset entities)
         */
        [HttpPut("{id}/execute")]
        public async Task<IActionResult> ExecuteChangePlan(Guid id)
        {
            var changePlanItems = await _changePlanService.GetChangePlanItemsAsync(id);
            //waiting to see if the change plan successfully finished
            /*
             * TODO: joyce, just use a single method IChangePlanService.ExecuteChangePlanAsync(ChangePlanDto changePlan)
             * TODO: ... instead of taking a List<ChangePlanItem> parameter.
             * TODO: ... set ChangePlan.ExecutedDate *within the service method*
             */
            await _changePlanService.ExecuteChangePlan(changePlanItems);

            //setting a new execute date for the change plan
            // TODO: see above, this can go away. Fewer database hits = better,
            // TODO: ... and technically by doing it in two steps like this, you can accidentally
            // TODO: ... leave the database in an inconsistent state, with the assets overwritten but the change plan not marked executed.
            var changePlan = await _changePlanService.GetChangePlanAsync(id);
            changePlan.ExecutedDate = DateTime.Now;
            await _changePlanService.UpdateChangePlanAsync(changePlan);

            /*
             * TODO: In summary:
             *  - var changePlan = _changePlanService.GetChangePlanAsync(id)
             *  - _changePlanService.ExecuteChangePlanAsync(changePlan)
             *  - return Ok()
             */

            // Oh, and update ChangePlanDto to have a list of ChangePlanItems! this is critical

            return Ok();
        }
    }
}