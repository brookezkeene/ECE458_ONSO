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
                foreach (ChangePlanItemDto changePlanItem in changePlanItems)
                {
                    var changePlanAssetDto = new AssetDto();
                    var blades = new List<GetAssetsApiDto>();
                    if (changePlanItem.ExecutionType.Equals("decommission"))
                    {
                        var oldAsset = response.Find(x => x.Id == changePlanItem.AssetId);
                        response.Remove(oldAsset);
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
                        var oldAsset = response.Find(x => x.Id == changePlanItem.AssetId);
                        if (oldAsset != null && oldAsset.Blades != null && oldAsset.Blades.Count != 0) blades = (oldAsset.Blades);
                        response.Remove(oldAsset);
                    }
                    changePlanAssetDto = await _changePlanService.FillFieldsInAssetApiForChangePlans(changePlanAssetDto);
                    var assetApiDto = _mapper.Map<GetAssetApiDto>(changePlanAssetDto);
                    assetApiDto.Blades = blades;
                    if (changePlanAssetDto.ChassisId != null && changePlanAssetDto.ChassisId != Guid.Empty) //only happens when updating a blade
                    {
                        var oldChassisId = changePlanAssetDto.ChassisId ?? Guid.Empty;
                        if (!string.IsNullOrEmpty(changePlanItem.PreviousData))
                            oldChassisId = (JsonConvert.DeserializeObject<GetAssetApiDto>(changePlanItem.NewData)).Id;
                        var chassis = response.Find(x => x.Id == oldChassisId);
                        //remove previous blade
                        if (chassis != null && chassis.Blades != null)
                        {
                            chassis.Blades.Remove(chassis.Blades.Find(x => x.Id == changePlanAssetDto.Id));
                            chassis.Blades.Add(assetApiDto);
                        }
                        continue;
                    }
                    response.Add(assetApiDto);
                    response.TotalCount++;
                }
            }

            return Ok(response);
        }

        [HttpGet("asset")]
        public async Task<ActionResult<GetAssetApiDto>> GetById([FromQuery] GetAssetByIdQuery query)
        {

            var changePlanItem = await _changePlanService.GetChangePlanItemAsync(query.AssetId);
            if (changePlanItem == null)
            {
                changePlanItem = await _changePlanService.GetChangePlanItemAsync(query.ChangePlanId ?? Guid.Empty, query.AssetId);
            }
            //check to see if you're in a change plan               
            if (query.ChangePlanId != null && query.ChangePlanId != Guid.Empty && changePlanItem != null)
            {
                //if the change plan item is a decommissioned asset
                if (changePlanItem.ExecutionType.Equals("decommission"))
                {
                    var decommissionedAssetDto = (JsonConvert.DeserializeObject<DecommissionedAssetDto>(changePlanItem.NewData));
                    var decommission = JsonConvert.DeserializeObject<CreateDecommissionedAsset>(decommissionedAssetDto.Data);
                    return Ok(decommission);
                }

                //if the change plan item is a created or updated item
                var assetDto = new AssetDto();
                if (changePlanItem.ExecutionType.Equals("create"))
                {
                    assetDto = _mapper.Map<AssetDto>(JsonConvert.DeserializeObject<CreateAssetApiDto>(changePlanItem.NewData));
                    assetDto.Id = changePlanItem.Id;
                }
                else if (changePlanItem.ExecutionType.Equals("update"))
                {
                    assetDto = _mapper.Map<AssetDto>(JsonConvert.DeserializeObject<UpdateAssetApiDto>(changePlanItem.NewData));
                }
                await _changePlanService.FillFieldsInAssetApiForChangePlans(assetDto);
                await AddBladesToGetCall(query, assetDto);
                return Ok(_mapper.Map<GetAssetApiDto>(assetDto));
            }
            var asset = await _assetService.GetAssetAsync(query.AssetId);
            var decommissionedAsset = await _assetService.GetDecommissionedAssetAsync(query.AssetId);
            if (asset != null)
            {
                await AddBladesToGetCall(query, asset);
                var response = _mapper.Map<GetAssetApiDto>(asset);               
                return Ok(response);
            }
            else if (decommissionedAsset != null)
            {
                var assetApi = JsonConvert.DeserializeObject<CreateDecommissionedAsset>(decommissionedAsset.Data);
                return Ok(assetApi);
            }

            //nothign was found 
            return Ok();
        }

        [HttpGet("asset/{assetNumber}")]
        public async Task<ActionResult<GetAssetApiDto>> GetByNumber(int assetNumber)
        {
            var asset = await _assetService.GetAssetByNumber(assetNumber);

            //is no asset is returned, may be a decommissioned asset
            if (asset != null)
            {
                var response = _mapper.Map<GetAssetApiDto>(asset);
                return Ok(response);
            }
            //nothign was found 
            return Ok();
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
                var newData = JsonConvert.SerializeObject(assetApiDto);
                createdId = await _changePlanService.CreateChangePlanItemAsync(changePlanId, newData);
            }
            else
            {
                createdId = await _assetService.CreateAssetAsync(assetDto);
            }

            var createdAssetDto = await _assetService.GetAssetAsync(createdId);
            var response = _mapper.Map<GetAssetApiDto>(createdAssetDto);

            return CreatedAtAction(nameof(GetById), new { id = createdId }, response);
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
                //this is when you are updating a change plan's created asset
                if (originalAsset == null)
                {
                    var createdItem = await _changePlanService.GetChangePlanItemAsync(changePlanId, assetApiDto.Id);
                    var newAsset = _mapper.Map<CreateAssetApiDto>(assetApiDto);
                    string newData = JsonConvert.SerializeObject(newAsset);
                    createdItem.NewData = newData;
                    await _changePlanService.UpdateChangePlanItemAsync(createdItem);
                    return NoContent();
                }
                var newInfo = JsonConvert.SerializeObject(assetApiDto);
                var oldInfo = JsonConvert.SerializeObject(originalAsset);
                await _changePlanService.CreateChangePlanItemAsync(changePlanId, assetApiDto.Id, newInfo, oldInfo);

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
                var decommissionedAsset = CreateDecommissionedAsset(assetDto, query);
                //check to see if it's a blade chassis, if yes, decommission + delete all the blades within it too
                //decommissionedAsset already has the information of all the blades in it already
                await DecommissionBladeChassis(assetDto, query);
                //deleting asset from active asset column + adding the decommissioned asset to the table
                
                await _assetService.CreateDecommissionedAssetAsync(decommissionedAsset);
                await _assetService.DeleteAssetAsync(query.Id);
            }

            if (query.ChangePlanId != null && query.ChangePlanId != Guid.Empty)
            {
                var updatedAssetChangePlan = await _changePlanService.GetChangePlanItemAsync(query.ChangePlanId ?? Guid.Empty, query.Id);
                //checking to see if this asset has been created/updated in the change plan 
                //using this info to create and display the new decommissioned asset
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
                    if (updateadAssetDto.Model.MountType.Equals("chassis"))
                    {
                        await DecommissionBladeChangePlan(updateadAssetDto, query);
                    }
                    decommissionedAsset = CreateDecommissionedAsset(updateadAssetDto, query);

                }
                //this asset to be decommissioned was not found in the change assets items and 
                //it has been found in the assets data
                else if (assetDto != null)
                {
                    if (assetDto.Model.MountType.Equals("chassis"))
                    {
                        await DecommissionBladeChangePlan(assetDto, query);
                    }
                    decommissionedAsset = CreateDecommissionedAsset(assetDto, query);
                }
                await _changePlanService.CreateChangePlanItemAsync(query.ChangePlanId ?? Guid.Empty, query.Id, decommissionedAsset);
            }

            return Ok();
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
                foreach (ChangePlanItemDto changePlanItem in decommissionedChangePlans)
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

        //TODO: May want to put this somewhere else
        private DecommissionedAssetDto CreateDecommissionedAsset(AssetDto assetDto, DecommissionedAssetQuery query)
        {
            //creating the createDecommissionedAsset from the assetDto
            var createDecommissionedAsset = _mapper.Map<CreateDecommissionedAsset>(assetDto);
            //adding network graph, person who decommissioned, and date decommissioned to decommissioned asset info
            createDecommissionedAsset.NetworkPortGraph = query.NetworkPortGraph;
            createDecommissionedAsset.Decommissioner = query.Decommissioner;
            createDecommissionedAsset.DateDecommissioned = DateTime.Now;
            var jsonString = JsonConvert.SerializeObject(createDecommissionedAsset);
            //creating a new decommissionedAssetDto + filling in the data
            var decommissionedAsset = _mapper.Map<DecommissionedAssetDto>(createDecommissionedAsset);
            decommissionedAsset.Data = jsonString;
            return decommissionedAsset;
        }
        private async Task<ActionResult> DecommissionBladeChassis(AssetDto assetDto, DecommissionedAssetQuery query)
        {
            if (assetDto.Blades == null) return Ok();
            foreach (AssetDto blade in assetDto.Blades)
            {
                var decommissionedAsset = CreateDecommissionedAsset(blade, query);
                await _assetService.DeleteAssetAsync(blade.Id);
                await _assetService.CreateDecommissionedAssetAsync(decommissionedAsset);
            }
            return Ok();
        }
        private async Task<ActionResult> DecommissionBladeChangePlan(AssetDto chassis, DecommissionedAssetQuery query)
        {
            var changePlanId = query.ChangePlanId ?? Guid.Empty;
            var changePlanItems = await _changePlanService.GetChangePlanItemsAsync(changePlanId);
            var assetDto = await _assetService.GetAssetAsync(chassis.Id);
            //decommissioning all the current blades in the chassis 
            if (assetDto!= null && assetDto.Blades != null)
            {
                foreach (AssetDto blade in assetDto.Blades)
                {
                    var item = await _changePlanService.GetChangePlanItemAsync(query.ChangePlanId ?? Guid.Empty, blade.Id);
                    if (item != null) { continue; } //if item != null, there's an updated version which exists and will be decommissioned below
                    var decommissionedAsset = CreateDecommissionedAsset(blade, query);
                    await _changePlanService.CreateChangePlanItemAsync(changePlanId, blade.Id, decommissionedAsset);
                }
            }
            //decommissioning the rest of the blades (in the change plan items)
            foreach (ChangePlanItemDto changePlanItem in changePlanItems)
            {
                var newAssetDto = new AssetDto();
                if (changePlanItem.ExecutionType.Equals("create"))
                {
                    var updatedAssetApi = JsonConvert.DeserializeObject<CreateAssetApiDto>(changePlanItem.NewData);
                    newAssetDto = _mapper.Map<AssetDto>(updatedAssetApi);
                }
                else if (changePlanItem != null && changePlanItem.ExecutionType.Equals("update"))
                {
                    var updatedAssetApi = JsonConvert.DeserializeObject<UpdateAssetApiDto>(changePlanItem.NewData);
                    newAssetDto = _mapper.Map<AssetDto>(updatedAssetApi);
                }
                if (newAssetDto.ChassisId != query.Id) //this change plan item is not part of the chassis' blades
                {
                    continue;
                }
                //var decommissionAssetExists = _changePlanService.GetChangePlanItemAsync(newAssetDto.Id);
                var updateadAssetDto = await _changePlanService.FillFieldsInAssetApiForChangePlans(newAssetDto);
                var decommissionedAsset = CreateDecommissionedAsset(updateadAssetDto, query);
                Guid assetId = changePlanItem.AssetId;
                if (assetId == Guid.Empty) assetId = changePlanItem.Id;
                await _changePlanService.CreateChangePlanItemAsync(changePlanId, assetId, decommissionedAsset);
            }
            return Ok();
        }
        private async Task<ActionResult> AddBladesToGetCall(GetAssetByIdQuery query, AssetDto chassis)
        {
            if (!chassis.Model.MountType.Contains("chassis") || query.ChangePlanId == null || query.ChangePlanId==Guid.Empty) return Ok();
            var changePlanId = query.ChangePlanId ?? Guid.Empty;
            var changePlanItems = await _changePlanService.GetChangePlanItemsAsync(changePlanId);
            foreach (ChangePlanItemDto changePlanItem in changePlanItems)
            {
                var newAssetDto = new AssetDto();
                if (changePlanItem.ExecutionType.Equals("create"))
                {
                    var updatedAssetApi = JsonConvert.DeserializeObject<CreateAssetApiDto>(changePlanItem.NewData);
                    newAssetDto = _mapper.Map<AssetDto>(updatedAssetApi);
                    newAssetDto.Id = changePlanItem.Id;
                }
                else if (changePlanItem != null && changePlanItem.ExecutionType.Equals("update"))
                {
                    var updatedAssetApi = JsonConvert.DeserializeObject<UpdateAssetApiDto>(changePlanItem.NewData);
                    newAssetDto = _mapper.Map<AssetDto>(updatedAssetApi);
                }
                if (newAssetDto.ChassisId != query.AssetId) //this change plan item is not part of the chassis' blades
                {
                    continue;
                }
                //var decommissionAssetExists = _changePlanService.GetChangePlanItemAsync(newAssetDto.Id);
                var updateadAssetDto = await _changePlanService.FillFieldsInAssetApiForChangePlans(newAssetDto);
                if (chassis.Blades != null)
                {
                    var remove = chassis.Blades.Find(x => x.Id == updateadAssetDto.Id);
                    chassis.Blades.Remove(remove);
                }
                chassis.Blades.Add(updateadAssetDto);
            }
            return Ok();
        }
    }
}
