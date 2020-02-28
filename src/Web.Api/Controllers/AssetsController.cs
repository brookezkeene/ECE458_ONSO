﻿using System;
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
        public async Task<ActionResult<PagedList<GetAssetsApiDto>>> GetMany(Guid? datacenterId, int page = 1, int pageSize = 10)
        {
            var assets = await _assetService.GetAssetsAsync(datacenterId, page, pageSize);

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

    }
}