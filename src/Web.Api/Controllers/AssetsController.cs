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
        private readonly IModelService _modelSerivce;
        private readonly IRackService _rackService;
        private readonly IIdentityService _userService;

        public AssetsController(IAssetService assetService, IMapper mapper, IApiErrorResources errorResources, PowerStateService powerStateService, 
            IChangePlanService changePlanService, IModelService modelService, IRackService rackService, IIdentityService userService)
        {
            _assetService = assetService;
            _mapper = mapper;
            _changePlanService = changePlanService;
            _powerStateService = powerStateService;
            _modelSerivce = modelService;
            _rackService = rackService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<GetAssetsApiDto>>> GetMany([FromQuery] SearchAssetQuery query)
        {
            var assets = await _assetService.GetAssetsAsync(query);
            var response = _mapper.Map<PagedList<GetAssetsApiDto>>(assets);

            if (query.ChangePlanId != null && query.ChangePlanId != Guid.Empty)
            {
                var changePlanId = query.ChangePlanId ?? Guid.Empty;
                var changePlanItems = await _changePlanService.GetChangePlanItemsAsync(changePlanId);
                var decommissionedChangePlans = await _changePlanService.GetDecommissionedChangePlanItemsAsync(changePlanId);
                List<GetAssetApiDto> changePlanAssetList = new List<GetAssetApiDto>();
                foreach (ChangePlanItemDto changePlanItem in changePlanItems)
                {
                    var changePlanAssetDto = new AssetDto();
                    if(changePlanItem.ExecutionType.Equals("decommission"))
                    {
                        var decommissionedAsset = response.Find(x => x.Id == changePlanItem.AssetId);
                        response.Remove(decommissionedAsset);
                        response.TotalCount -= 1;
                        continue;
                    }
                    else if (changePlanItem.ExecutionType.Equals("create"))
                    {
                         changePlanAssetDto = _mapper.Map<AssetDto>((JsonConvert.DeserializeObject<CreateAssetApiDto>(changePlanItem.NewData)));
                         changePlanAssetDto.Id = changePlanItem.Id;
                    }
                    else if (changePlanItem.ExecutionType.Equals("update"))
                    {
                        changePlanAssetDto = _mapper.Map<AssetDto>((JsonConvert.DeserializeObject<UpdateAssetApiDto>(changePlanItem.NewData)));
                        var updatedAsset = response.Find(x => x.Id == changePlanItem.AssetId);
                        response.Remove(updatedAsset);
                        if (decommissionedChangePlans.Find(x => x.AssetId == updatedAsset.Id) != null) { continue; }
                    }
                    changePlanAssetDto = await _changePlanService.FillFieldsInAssetApiForChangePlans(changePlanAssetDto);
                    changePlanAssetList.Add(_mapper.Map<GetAssetApiDto>(changePlanAssetDto));
                }
                response.AddRange(changePlanAssetList);
                response.TotalCount += changePlanAssetList.Count();
            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetAssetApiDto>> GetById(Guid id)
        {
            var asset = await _assetService.GetAssetAsync(id);
            //is no asset is returned, must be a decommissioned asset
            if(asset == null)
            {
                var assetDto = await _assetService.GetDecommissionedAssetAsync(id);
                //if no asset is returned, must be change plan item
                if(assetDto == null)
                {
                    return await GetByIdFromChangePlan(id);
                }
                var assetApi = JsonConvert.DeserializeObject<CreateDecommissionedAsset>(assetDto.Data);
                return Ok(assetApi);
            }
            var response = _mapper.Map<GetAssetApiDto>(asset);
            return Ok(response);
        }

        [HttpGet("{id}/changePlan")]
        public async Task<ActionResult<GetAssetApiDto>> GetByIdFromChangePlan(Guid id)
        {
            var item = await _changePlanService.GetChangePlanItemAsync(id);
            if (item == null)
            {
                return await GetById(id);
            }

            //if the change plan item is a decommissioned asset
            if (item.ExecutionType.Equals("decommission"))
            {
                var decommissionedAssetDto = (JsonConvert.DeserializeObject<DecommissionedAssetDto>(item.NewData));
                var decommission = JsonConvert.DeserializeObject<CreateDecommissionedAsset>(decommissionedAssetDto.Data);
                return Ok(decommission);
            }

            //if the change plan item is a created or updated item
            var assetDto = new AssetDto();
            if (item.ExecutionType.Equals("create"))
            {
                assetDto = _mapper.Map<AssetDto>(JsonConvert.DeserializeObject<CreateAssetApiDto>(item.NewData));                
            }
            else if (item.ExecutionType.Equals("update"))
            {
                assetDto = _mapper.Map<AssetDto>(JsonConvert.DeserializeObject<UpdateAssetApiDto>(item.NewData));
            }
            await _changePlanService.FillFieldsInAssetApiForChangePlans(assetDto);
            var response = _mapper.Map<GetAssetApiDto>(assetDto);
            return Ok(response);

        }


        /*
         * WHAT'S STORED IN THE CHANGEPLANITEM DATA: ApiDto or DecommissionedAssets
         */

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateAssetApiDto assetApiDto)
        {  
            var assetDto = _mapper.Map<AssetDto>(assetApiDto);

            Guid createdId;
            if (assetApiDto.ChangePlanId != null && assetApiDto.ChangePlanId != Guid.Empty)
            {
                var changePlanId = assetApiDto.ChangePlanId ?? Guid.Empty;
                var changePlanItemApiDto = new ChangePlanItemDto
                {
                    ChangePlanId = changePlanId,
                    ExecutionType = "create",
                    NewData = JsonConvert.SerializeObject(assetApiDto),
                    CreatedDate = DateTime.Now,
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

            //assetApiDto.LastUpdatedDate = DateTime.Now;

            if (assetApiDto.ChangePlanId != null && assetApiDto.ChangePlanId != Guid.Empty)
            {

                var changePlanId = assetApiDto.ChangePlanId ?? Guid.Empty;
                var originalAsset = _mapper.Map<GetAssetApiDto>(await _assetService.GetAssetAsync(assetApiDto.Id));
                if(originalAsset == null)
                {
                    var createdItem = await _changePlanService.GetChangePlanItemAsync(changePlanId, assetApiDto.Id);
                    var newAsset = _mapper.Map<CreateAssetApiDto>(assetApiDto);
                    string newData = JsonConvert.SerializeObject(newAsset);
                    createdItem.NewData = newData;
                    await _changePlanService.UpdateChangePlanItemAsync(createdItem);
                    return NoContent();
                }
                var changePlanItemApiDto = new ChangePlanItemDto
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
            if (assetDto != null && query.ChangePlanId == null)
            {
                var createDecommissionedAsset = _mapper.Map<CreateDecommissionedAsset>(assetDto);

                //adding network graph, person who decommissioned, and date decommissioned to decommissioned asset info

                createDecommissionedAsset.NetworkPortGraph = query.NetworkPortGraph; 
                createDecommissionedAsset.Decommissioner = query.Decommissioner;
                createDecommissionedAsset.DateDecommissioned = DateTime.Now;
                var jsonString = JsonConvert.SerializeObject(createDecommissionedAsset);

                //creating a new decommissionedAssetDto + filling in the data
                var decommissionedAsset = _mapper.Map<DecommissionedAssetDto>(createDecommissionedAsset);
                decommissionedAsset.Data = jsonString;

                //deleting asset from active asset column
                await _assetService.DeleteAssetAsync(query.Id);
                await _assetService.CreateDecommissionedAssetAsync(decommissionedAsset);
            }

            if (query.ChangePlanId != null && query.ChangePlanId != Guid.Empty)
            {
                var updatedAssetChangePlan = await _changePlanService.GetChangePlanItemAsync(query.ChangePlanId ?? Guid.Empty, query.Id);
                //checking to see if this asset has been created/updated in the change plan 
                //using this info to create and display the new decommissioned asset
                string jsonString = "new";
                var decommissionedAsset = new DecommissionedAssetDto();
                if (updatedAssetChangePlan != null)
                {
                    var newAssetDto = new AssetDto();
                    if (updatedAssetChangePlan.ExecutionType.Equals("create"))
                    {
                        var updatedAssetApi = JsonConvert.DeserializeObject<CreateAssetApiDto>(updatedAssetChangePlan.NewData);
                        newAssetDto = _mapper.Map<AssetDto>(updatedAssetApi);
                    }
                    else if (updatedAssetChangePlan != null && updatedAssetChangePlan.ExecutionType.Equals("update"))
                    {
                        var updatedAssetApi = JsonConvert.DeserializeObject<UpdateAssetApiDto>(updatedAssetChangePlan.NewData);
                        newAssetDto = _mapper.Map<AssetDto>(updatedAssetApi);
                    }
                    var updateadAssetDto = await _changePlanService.FillFieldsInAssetApiForChangePlans(newAssetDto);
                    var createDecommissionedAssetDto = _mapper.Map<CreateDecommissionedAsset>(updateadAssetDto);
                    createDecommissionedAssetDto.NetworkPortGraph = query.NetworkPortGraph;
                    createDecommissionedAssetDto.Decommissioner = query.Decommissioner;
                    createDecommissionedAssetDto.DateDecommissioned = DateTime.Now;
                    jsonString = JsonConvert.SerializeObject(createDecommissionedAssetDto);
                    decommissionedAsset = _mapper.Map<DecommissionedAssetDto>(createDecommissionedAssetDto);
                    decommissionedAsset.Data = jsonString;
                }
                else
                {
                    var createDecommissionedAsset = _mapper.Map<CreateDecommissionedAsset>(assetDto);

                    //adding network graph, person who decommissioned, and date decommissioned to decommissioned asset info

                    createDecommissionedAsset.NetworkPortGraph = query.NetworkPortGraph;
                    createDecommissionedAsset.Decommissioner = query.Decommissioner;
                    createDecommissionedAsset.DateDecommissioned = DateTime.Now;
                    jsonString = JsonConvert.SerializeObject(createDecommissionedAsset);

                    //creating a new decommissionedAssetDto + filling in the data
                    decommissionedAsset = _mapper.Map<DecommissionedAssetDto>(createDecommissionedAsset);
                    decommissionedAsset.Data = jsonString;
                }
                await _changePlanService.CreateChangePlanItemAsync(query.ChangePlanId ?? Guid.Empty, query.Id, decommissionedAsset, jsonString);
            }

            return Ok();
        }

        [HttpGet("{id}/decommission")]
        public async Task<ActionResult<DecommissionedAssetDto>> GetDecommissioned(Guid id)
        {
            var assetDto = await _assetService.GetDecommissionedAssetAsync(id);
            var assetApi = JsonConvert.DeserializeObject<CreateDecommissionedAsset>(assetDto.Data);
            return Ok(assetApi);
        }

        [HttpGet("decommission")]
        public async Task<ActionResult<PagedList<DecommissionedAssetDto>>> GetManyDecommissioned([FromQuery] SearchAssetQuery query)
        {
            var assets = await _assetService.GetDecommissionedAssetsAsync(query);

            var response = _mapper.Map<PagedList<DecommissionedAssetDto>>(assets);

            if (query.ChangePlanId != null && query.ChangePlanId != Guid.Empty)
            {
                var changePlanId = query.ChangePlanId ?? Guid.Empty;
                var decommissionedChangePlans = await _changePlanService.GetDecommissionedChangePlanItemsAsync(changePlanId);
                List<DecommissionedAssetDto> decommissionedAssets = new List<DecommissionedAssetDto>();
                foreach(ChangePlanItemDto changePlanItem in decommissionedChangePlans)
                {
                    var decommissionedAsset = JsonConvert.DeserializeObject<DecommissionedAssetDto>(changePlanItem.NewData);
                    //making sure decommissioned asset id is the change plan id for "getting" purposes
                    decommissionedAsset.Id = changePlanItem.Id;
                    decommissionedAssets.Add(decommissionedAsset);
                }
                response.AddRange(decommissionedAssets);
                response.TotalCount += decommissionedAssets.Count();
            }
            return Ok(response);
        }

    }
}
