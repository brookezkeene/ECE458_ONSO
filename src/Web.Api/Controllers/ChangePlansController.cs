using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos.Assets.Create;
using Web.Api.Dtos.Assets.Update;
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
        private readonly IAssetService _assetService;
        private readonly IMapper _mapper;


        public ChangePlansController(IChangePlanService changePlanService, IAssetService assetService, IMapper mapper)
        {
            _changePlanService = changePlanService;
            _assetService = assetService;
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
            changePlanDto.CreatedDate = DateTime.Now;
            await _changePlanService.CreateChangePlanAsync(changePlanDto);
            return Ok();
        }
        [HttpPost("changeplanitem")]
        public async Task<IActionResult> Post([FromBody] CreateChangePlanItemApiDto changePlanItem)
        {
            var changePlanItemDto = _mapper.Map<ChangePlanItemDto>(changePlanItem);
            changePlanItemDto.CreatedDate = DateTime.Now;
            await _changePlanService.CreateChangePlanItemAsync(changePlanItemDto);
            return Ok();
        }
        [HttpPut("changeplan")]
        public async Task<IActionResult> Put(ChangePlanDto changePlan)
        {
            await _changePlanService.UpdateChangePlanAsync(changePlan);
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
        [HttpPut("{id}/execute")]
        public async Task<IActionResult> ExecuteChangePlan(Guid id)
        {
            var changePlan = await _changePlanService.GetChangePlanAsync(id);
            changePlan.ExecutedDate = DateTime.Now; 
            await _changePlanService.UpdateChangePlanAsync(changePlan);
            var changePlanItems = await _changePlanService.GetChangePlanItemsAsync(id);
            for (int i = 0; i < changePlanItems.Count(); i++)
            {
                var changePlanItem = changePlanItems[i];
                //NOTE: THE NEWDATA HERE IS A CreateAssetApiDto
                if (changePlanItem.ExecutionType.Equals("create"))
                {
                    var assetApiDto = JsonConvert.DeserializeObject<CreateAssetApiDto>(changePlanItem.NewData);
                    //assetApiDto.LastUpdatedDate = DateTime.Now;
                    var assetDto = _mapper.Map<AssetDto>(assetApiDto);
                    await _assetService.CreateAssetAsync(assetDto);
                }
                //NOTE: THE NEWDATA HERE IS A UpdateAssetApiDto
                else if (changePlanItem.ExecutionType.Equals("update"))
                {
                    var assetApiDto = JsonConvert.DeserializeObject<UpdateAssetApiDto>(changePlanItem.NewData);
                    //assetApiDto.LastUpdatedDate = DateTime.Now;
                    var assetDto = _mapper.Map<AssetDto>(assetApiDto);
                    await _assetService.UpdateAssetAsync(assetDto);
                }
                //NOTE: THE NEWDATA HERE IS A DecommissionedAssetDto
                else if (changePlanItem.ExecutionType.Equals("decommission"))
                {
                    var decommisionedAsset = JsonConvert.DeserializeObject<DecommissionedAssetDto>(changePlanItem.NewData);
                    decommisionedAsset.DateDecommissioned = DateTime.Now;
                    await _assetService.DeleteAssetAsync(decommisionedAsset.Id);
                    await _assetService.CreateDecommissionedAssetAsync(decommisionedAsset);
                }
            }

            return Ok();
        }

    }
}