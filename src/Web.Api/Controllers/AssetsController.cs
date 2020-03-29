using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Dtos.Power;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos;
using Web.Api.Dtos.Assets;
using Web.Api.Dtos.Assets.Create;
using Web.Api.Dtos.Assets.Read;
using Web.Api.Dtos.Assets.Update;
using Web.Api.Dtos.ChangePlans;
using Web.Api.Mappers;
using Web.Api.Resources;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _assetService;
        private readonly IChangePlanService _changePlanService;
        private readonly IModelService _modelSerivce;
        private readonly IRackService _rackService;

        private readonly IApiErrorResources _errorResources;
        private readonly PowerService _powerService;

        public AssetsController(IAssetService assetService, IApiErrorResources errorResources, PowerService powerService, 
            IChangePlanService changePlanService, IModelService modelService, IRackService rackService)
        {
            _assetService = assetService;
            _errorResources = errorResources;
            _powerService = powerService;
            _changePlanService = changePlanService;
            _modelSerivce = modelService;
            _rackService = rackService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<GetAssetsApiDto>>> GetMany([FromQuery] SearchAssetQuery query)
        {
            var assets = await _assetService.GetAssetsAsync(query);
            if(query.ChangePlanId != null && query.ChangePlanId != Guid.Empty)
            {

            }
            var response = assets.MapTo<PagedList<GetAssetsApiDto>>();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetAssetApiDto>> Get(Guid id)
        {
            var asset = await _assetService.GetAssetAsync(id);

            var response = asset.MapTo<GetAssetApiDto>();
            return Ok(response);
        }

        /*
         * WHAT'S STORED IN THE CHANGEPLANITEM DATA: it's either an Asset entity
         * or a DecommissionedAsset entity
         */
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAssetApiDto assetApiDto)
        {  
            assetApiDto.LastUpdatedDate = DateTime.Now;
            var assetDto = assetApiDto.MapTo<AssetDto>();
            if (assetApiDto.ChangePlanId != null && assetApiDto.ChangePlanId != Guid.Empty)
            {
                var changePlanId = assetApiDto.ChangePlanId ?? Guid.Empty;
                var changePlanItemApiDto = new CreateChangePlanItemApiDto
                {
                    ChangePlanId = changePlanId,
                    ExecutionType = "create",
                    NewData = JsonConvert.SerializeObject(assetApiDto),
                    CreatedDate = DateTime.Now
                };
                var changePlanItemDto = changePlanItemApiDto.MapTo<ChangePlanItemDto>();
                await _changePlanService.CreateChangePlanItemAsync(changePlanItemDto);
            }
            else
            {
                await _assetService.CreateAssetAsync(assetDto);
            }

            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var asset = await _assetService.GetAssetAsync(id);
            await _assetService.DeleteAssetAsync(asset);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Put(UpdateAssetApiDto assetApiDto)
        {
            
            assetApiDto.LastUpdatedDate = DateTime.Now;
            var assetDto = assetApiDto.MapTo<AssetDto>();
            if (assetApiDto.ChangePlanId != null && assetApiDto.ChangePlanId != Guid.Empty)
            {
                var changePlanId = assetApiDto.ChangePlanId ?? Guid.Empty;
                var originalAsset = (await _assetService.GetAssetAsync(assetApiDto.Id)).MapTo<GetAssetApiDto>();
                var changePlanItemApiDto = new UpdateChangePlanItemApiDto
                {
                    ChangePlanId = changePlanId,
                    ExecutionType = "update",
                    AssetId = assetApiDto.Id,
                    NewData = JsonConvert.SerializeObject(assetApiDto),
                    PreviousData = JsonConvert.SerializeObject(originalAsset),
                    CreatedDate = DateTime.Now
                };
                var changePlanItemDto = changePlanItemApiDto.MapTo<ChangePlanItemDto>();
                await _changePlanService.CreateChangePlanItemAsync(changePlanItemDto);
            }
            else
            {
                await _assetService.UpdateAssetAsync(assetDto);
            }
            return NoContent();
        }

        [HttpGet("{id}/power")]
        public async Task<ActionResult<GetAssetPowerStateApiDto>> GetPowerState(Guid id)
        
        {
            var resp = await _powerService.getStates(id);
            var response = resp.MapTo<GetAssetPowerStateApiDto>();

            return Ok(response);
        }

        [HttpPut("{id}/power")]
        public async Task<ActionResult<GetAssetPowerStateApiDto>> PostPowerState(Guid id,
            [FromBody] UpdateAssetPowerStateApiDto powerState)
        {
            var state = powerState.Action;
            // Call to update the power state of the associated asset ports to on/off/cycle

            var resp = await _powerService.setStates(id, state);
            var response = resp.MapTo<GetAssetPowerStateApiDto>();

            return Ok(response);
        }

        [HttpPost("decommission")]
        public async Task<IActionResult> Post([FromQuery] DecommissionedAssetQuery query)
        {
            var assetDto = await _assetService.GetAssetForDecommissioning(query.Id);
            var createDecommissionedAsset  = assetDto.MapTo<CreateDecommissionedAsset>();

            //adding network graph to the asset
            createDecommissionedAsset.NetworkPortGraph = query.NetworkPortGraph;

            //creating a new decommissionedAssetDto from assetDto, filling in the data, decommissioner, and date
            var jsonString = JsonConvert.SerializeObject(createDecommissionedAsset);
            var decommisionedAsset = assetDto.MapTo<DecommissionedAssetDto>();
            decommisionedAsset.Data = jsonString;
            decommisionedAsset.Decommissioner = query.Decommissioner;
            decommisionedAsset.DateDecommissioned = DateTime.Now;

            if (query.ChangePlanId != null && query.ChangePlanId != Guid.Empty)
            {
                var changePlanId = query.ChangePlanId ?? Guid.Empty;
                var changePlanItemApiDto = new UpdateChangePlanItemApiDto
                {
                    ChangePlanId = changePlanId,
                    ExecutionType = "update",
                    AssetId = query.Id,
                    NewData = JsonConvert.SerializeObject(decommisionedAsset),
                    PreviousData = JsonConvert.SerializeObject(createDecommissionedAsset),
                    CreatedDate = DateTime.Now
                };
                var changePlanItemDto = changePlanItemApiDto.MapTo<ChangePlanItemDto>();
                await _changePlanService.CreateChangePlanItemAsync(changePlanItemDto);
            }
            else
            {
                var asset = await _assetService.GetAssetAsync(query.Id);
                await _assetService.CreateDecommissionedAssetAsync(decommisionedAsset);
                //deleting asset from active asset column
                await _assetService.DeleteAssetAsync(asset);
            }

            return Ok();
        }
        [HttpGet("{id}/decommission")]
        public async Task<ActionResult<DecommissionedAssetDto>> GetDecommissioned(Guid id)
        {
            var asset = await _assetService.GetDecommissionedAssetAsync(id);
            return Ok(asset);
        }
        [HttpGet("decommission")]
        public async Task<ActionResult<PagedList<DecommissionedAssetDto>>> GetManyDecommissioned([FromQuery] SearchAssetQuery query)
        {
            var assets = await _assetService.GetDecommissionedAssetsAsync(query);

            var response = assets.MapTo<PagedList<DecommissionedAssetDto>>();
            return Ok(response);
        }
    }
}
