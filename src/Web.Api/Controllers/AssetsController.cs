using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Renci.SshNet;
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
                foreach (ChangePlanItemDto changePlanItem in changePlanItems)
                {
                    var changePlanAssetDto = new AssetDto();
                    if (changePlanItem.ExecutionType.Equals("decommission"))
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
                    response.Add(_mapper.Map<GetAssetApiDto>(changePlanAssetDto));
                    response.TotalCount++;
                }

            }

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetAssetApiDto>> GetById(Guid id)
        {
            var asset = await _assetService.GetAssetAsync(id);
            var decommissionedAsset = await _assetService.GetDecommissionedAssetAsync(id);
            var changePlanItem = await _changePlanService.GetChangePlanItemAsync(id);
            //is no asset is returned, may be a decommissioned asset
            if (asset != null)
            {
                var response = _mapper.Map<GetAssetApiDto>(asset);
                return Ok(response);
            }
            else if (decommissionedAsset != null)
            {
                var assetApi = JsonConvert.DeserializeObject<CreateDecommissionedAsset>(decommissionedAsset.Data);
                return Ok(assetApi);
            }
            //if no asset is returned, may be change plan item                   
            else if (changePlanItem != null)
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
                }
                else if (changePlanItem.ExecutionType.Equals("update"))
                {
                    assetDto = _mapper.Map<AssetDto>(JsonConvert.DeserializeObject<UpdateAssetApiDto>(changePlanItem.NewData));
                }
                await _changePlanService.FillFieldsInAssetApiForChangePlans(assetDto);
                return Ok(_mapper.Map<GetAssetApiDto>(assetDto));
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
                if (originalAsset == null)
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
                var decommissionedAsset = CreateDecommissionedAsset(assetDto, query);
                //check to see if it's a blade chassis, if yes, decommission + delete all the blades within it too
                //decommissionedAsset already has the information of all the blades in it already
                await DecommissionBladeChassis(assetDto, query);
                //deleting asset from active asset column + adding the decommissioned asset to the table
                await _assetService.DeleteAssetAsync(query.Id);
                await _assetService.CreateDecommissionedAssetAsync(decommissionedAsset);
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
                    decommissionedAsset = CreateDecommissionedAsset(updateadAssetDto, query);

                }
                //this asset to be decommissioned was not found in the change assets items and 
                //it has been found in the assets data
                else if (assetDto != null)
                {
                    decommissionedAsset = CreateDecommissionedAsset(assetDto, query);
                }
                await _changePlanService.CreateChangePlanItemAsync(query.ChangePlanId ?? Guid.Empty, query.Id, decommissionedAsset);
            }

            return Ok();
        }


        [HttpGet("decommission")]
        public async Task<ActionResult<PagedList<DecommissionedAssetDto>>> GetManyDecommissioned([FromQuery] SearchAssetQuery query)
        {
            SshClient sshclient = new SshClient("hyposoft-mgt.colab.duke.edu", 2222, "admin3", "TSfS#458");
            sshclient.Connect();
            sshclient.ConnectionInfo.Timeout = TimeSpan.FromSeconds(1000);
            SshCommand sc = sshclient.CreateCommand("chassis myChassis1");
            sc.Execute();
            sc = sshclient.CreateCommand("power");
            sc.Execute();
            sshclient.Disconnect();
            

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

            foreach (AssetDto blade in assetDto.Blades)
            {
                var decommissionedAsset = CreateDecommissionedAsset(blade, query);
                await _assetService.DeleteAssetAsync(blade.Id);
                await _assetService.CreateDecommissionedAssetAsync(decommissionedAsset);
            }
            return Ok();
        }
        //CHECK HERE TO SEE IF BLADES HAVE IDS - IF SO, ARE THE IDS THE CHANGE PLAN ITEM IDS?
       /* private async Task<ActionResult> DecommissionBladeChassisChangePlan(AssetDto assetDto, DecommissionedAssetQuery query)
        {
            while (assetDto.Blades != null && assetDto.Blades.Count != 0)
            {
                var blade = await _assetService.GetAssetAsync(assetDto.Blades.First().Id);
                if (blade == null) {
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
                }
            }
        }*/
    }
}
