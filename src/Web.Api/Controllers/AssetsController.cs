using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IChangePlanService _changePlanService;
        private readonly PowerStateService _powerStateService;

        public AssetsController(IAssetService assetService, IMapper mapper, IChangePlanService changePlanService, PowerStateService powerStateService)
        {
            _assetService = assetService;
            _mapper = mapper;
            _changePlanService = changePlanService;
            _powerStateService = powerStateService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<GetAssetsApiDto>>> GetMany([FromQuery] SearchAssetQuery query)
        {
            var assets = await _assetService.GetAssetsAsync(query);
            if (query.ChangePlanId != null && query.ChangePlanId != Guid.Empty)
            {

            }

            var response = _mapper.Map<PagedList<GetAssetsApiDto>>(assets);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetAssetApiDto>> GetById(Guid id)
        {
            var asset = await _assetService.GetAssetAsync(id);

            var response = _mapper.Map<GetAssetApiDto>(asset);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAssetApiDto assetApiDto)
        {  
            var assetDto = _mapper.Map<AssetDto>(assetApiDto);

            Guid createdId;
            if (assetApiDto.ChangePlanId != null && assetApiDto.ChangePlanId != Guid.Empty)
            {
                var changePlanId = assetApiDto.ChangePlanId ?? Guid.Empty;
                var changePlanItemApiDto = new CreateChangePlanItemApiDto
                {
                    ChangePlanId = changePlanId,
                    ExecutionType = "create",
                    NewData = JsonConvert.SerializeObject(assetApiDto)
                };
                var changePlanItemDto = _mapper.Map<ChangePlanItemDto>(changePlanItemApiDto);
                createdId = await _changePlanService.CreateChangePlanItemAsync(changePlanItemDto);
            }
            else
            {
                createdId = await _assetService.CreateAssetAsync(assetDto);
            }

            var createdAssetDto = await _assetService.GetAssetAsync(createdId);
            var response = _mapper.Map<GetAssetApiDto>(createdAssetDto);

            return CreatedAtAction(nameof(GetById), new {id = createdId}, response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _assetService.DeleteAssetAsync(id);
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateAssetApiDto assetApiDto)
        {
            var assetDto = _mapper.Map<AssetDto>(assetApiDto);

            if (assetApiDto.ChangePlanId != null && assetApiDto.ChangePlanId != Guid.Empty)
            {
                var changePlanId = assetApiDto.ChangePlanId ?? Guid.Empty;
                var originalAsset = _mapper.Map<GetAssetApiDto>(await _assetService.GetAssetAsync(assetApiDto.Id));
                var changePlanItemApiDto = new UpdateChangePlanItemApiDto
                {
                    ChangePlanId = changePlanId,
                    ExecutionType = "update",
                    AssetId = assetApiDto.Id,
                    NewData = JsonConvert.SerializeObject(assetApiDto),
                    PreviousData = JsonConvert.SerializeObject(originalAsset),
                    CreatedDate = DateTime.Now
                };
                var changePlanItemDto = _mapper.Map<ChangePlanItemDto>(changePlanItemApiDto);
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
            var resp = await _powerStateService.GetPowerStateAsync(id);
            var response = _mapper.Map<GetAssetPowerStateApiDto>(resp);

            return Ok(response);
        }

        [HttpPut("{id}/power")]
        public async Task<ActionResult<GetAssetPowerStateApiDto>> PostPowerState(Guid id,
            [FromBody] UpdateAssetPowerStateApiDto powerState)
        {
            var state = powerState.Action;
            // Call to update the power state of the associated asset ports to on/off/cycle

            var resp = await _powerStateService.UpdatePowerStateAsync(id, state);
            var response = _mapper.Map<GetAssetPowerStateApiDto>(resp);

            return Ok(response);
        }

        [HttpPost("decommission")]
        public async Task<IActionResult> Post([FromQuery] DecommissionedAssetQuery query)
        {
            var assetDto = await _assetService.GetAssetForDecommissioning(query.Id);
            var createDecommissionedAsset  = _mapper.Map<CreateDecommissionedAsset>(assetDto);

            //adding network graph to the asset
            createDecommissionedAsset.NetworkPortGraph = query.NetworkPortGraph;

            //creating a new decommissionedAssetDto from assetDto, filling in the data, decommissioner, and date
            var decommissionedAsset = _mapper.Map<DecommissionedAssetDto>(assetDto);

            var jsonString = JsonConvert.SerializeObject(createDecommissionedAsset);
            decommissionedAsset.Data = jsonString;
            decommissionedAsset.Decommissioner = query.Decommissioner;
            decommissionedAsset.DateDecommissioned = DateTime.Now;

            if (query.ChangePlanId != null && query.ChangePlanId != Guid.Empty)
            {
                var changePlanId = query.ChangePlanId ?? Guid.Empty;
                var changePlanItemApiDto = new UpdateChangePlanItemApiDto
                {
                    ChangePlanId = changePlanId,
                    ExecutionType = "update",
                    AssetId = query.Id,
                    NewData = JsonConvert.SerializeObject(decommissionedAsset),
                    PreviousData = JsonConvert.SerializeObject(createDecommissionedAsset),
                    CreatedDate = DateTime.Now
                };
                var changePlanItemDto = _mapper.Map<ChangePlanItemDto>(changePlanItemApiDto);
                await _changePlanService.CreateChangePlanItemAsync(changePlanItemDto);
            }
            else
            {
                var asset = await _assetService.GetAssetAsync(query.Id);
                await _assetService.CreateDecommissionedAssetAsync(decommissionedAsset);
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

            var response = _mapper.Map<PagedList<DecommissionedAssetDto>>(assets);
            return Ok(response);
        }
    }
}
