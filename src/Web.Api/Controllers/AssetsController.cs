using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Dtos.Power;
using Web.Api.Core.Services;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos;
using Web.Api.Dtos.Assets;
using Web.Api.Dtos.Assets.Create;
using Web.Api.Dtos.Assets.Read;
using Web.Api.Dtos.Assets.Update;
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
        private readonly IApiErrorResources _errorResources;
        private readonly PowerService _powerService;

        public AssetsController(IAssetService assetService, IApiErrorResources errorResources, PowerService powerService)
        {
            _assetService = assetService;
            _errorResources = errorResources;
            _powerService = powerService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<GetAssetsApiDto>>> GetMany([FromQuery] SearchAssetQuery query)
        {
            var assets = await _assetService.GetAssetsAsync(query);

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


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAssetApiDto assetApiDto)
        {
            var assetDto = assetApiDto.MapTo<AssetDto>();
            await _assetService.CreateAssetAsync(assetDto);

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
            var assetDto = assetApiDto.MapTo<AssetDto>();
            await _assetService.UpdateAssetAsync(assetDto);
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
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(createDecommissionedAsset);
            var decommisionedAsset = assetDto.MapTo<DecommissionedAssetDto>();
            decommisionedAsset.Data = jsonString;
            decommisionedAsset.Decommissioner = query.Decommissioner;
            decommisionedAsset.Date = DateTime.Now.ToString("yyyy-MM-dd");

            //deleting asset from active asset column
            var asset = await _assetService.GetAssetAsync(query.Id);

            await _assetService.CreateDecommissionedAssetAsync(decommisionedAsset);

            await _assetService.DeleteAssetAsync(asset);

            return Ok();
        }
        [HttpGet("{id}/decommission")]
        public async Task<ActionResult<DecommissionedAssetDto>> GetDecommissioned(Guid id)
        {
            var asset = await _assetService.GetDecommissionedAssetAsync(id);
            return Ok(asset);
        }
        [HttpGet("decommission")]
        public async Task<ActionResult<PagedList<DecommissionedAssetDto>>> GetManyDecommissioned(int page = 1, int pageSize = 10)
        {
            var assets = await _assetService.GetDecommissionedAssetsAsync( page, pageSize);

            var response = assets.MapTo<PagedList<DecommissionedAssetDto>>();
            return Ok(response);
        }
    }
}
