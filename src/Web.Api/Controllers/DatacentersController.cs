using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Web.Api.Common;
using Web.Api.Core;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos;
using Web.Api.Dtos.Assets.Create;
using Web.Api.Dtos.Assets.Read;
using Web.Api.Dtos.Assets.Update;
using Web.Api.Dtos.Datacenters;
using Web.Api.Dtos.Datacenters.Create;
using Web.Api.Dtos.Datacenters.Read;
using Web.Api.Dtos.Datacenters.Update;
using Web.Api.Mappers;
using Web.Api.Resources;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class DatacentersController : ControllerBase
    {
        private readonly IDatacenterService _datacenterService;
        private readonly IApiErrorResources _errorResources;
        private readonly IChangePlanService _changePlanService;
        private readonly IMapper _mapper;

        public DatacentersController(IDatacenterService datacenterService, IApiErrorResources errorResources, IMapper mapper, IChangePlanService changePlanService)
        {
            _datacenterService = datacenterService;
            _errorResources = errorResources;
            _changePlanService = changePlanService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<GetDatacenterApiDto>>> GetMany(string searchText, int page = 1, int pageSize = 10)
        {
            var datacenters = await _datacenterService.GetDatacentersAsync(searchText, page, pageSize);
            var response = _mapper.Map<PagedList<GetDatacenterApiDto>>(datacenters);
            return Ok(response);
        }


        [HttpGet("{id}/networkports")]
        public async Task<ActionResult<List<GetAssetNetworkPortFromDatacenterDto>>> GetNetworks(Guid id)
        {
            var ports = await _datacenterService.GetNetworkPortsOfDataCenterAsync(id);
            var response = _mapper.Map<List<GetAssetNetworkPortFromDatacenterDto>>(ports);
            return Ok(response);
        }
        [HttpGet("chassis")]
        public async Task<ActionResult<List<GetAssetApiDto>>> GetChassis([FromQuery] GetAssetByIdQuery query)
        {
            var assets = await _datacenterService.GetChassisOfDataCenterAsync(query.AssetId);
            var response = _mapper.Map<List<GetAssetsApiDto>>(assets);
            if (query.ChangePlanId != null && query.ChangePlanId != Guid.Empty)
            {
                await GetChangePlanChassis(response, query.ChangePlanId?? Guid.Empty);
            }
            return Ok(response);
        }

        [HttpGet("{id}/networkports/open")]
        public async Task<ActionResult<List<GetAssetNetworkPortFromDatacenterDto>>> GetOpenNetworkPorts(Guid id)
        {
            var ports = await _datacenterService.GetOpenNetworkPortsOfDatacenterAsync(id);
            var response = _mapper.Map<List<GetAssetNetworkPortFromDatacenterDto>>(ports);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetDatacenterApiDto>> Get(Guid id)
        {
            var datacenter = await _datacenterService.GetDatacenterAsync(id);
            var response = _mapper.Map<GetDatacenterApiDto>(datacenter);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateDatacenterApiDto datacenterApiDto)
        {
            var datacenterDto = _mapper.Map<DatacenterDto>(datacenterApiDto);
            await _datacenterService.UpdateDatacenterAsync(datacenterDto);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateDatacenterApiDto datacenterApiDto)
        {
            var datacenterDto = _mapper.Map<DatacenterDto>(datacenterApiDto);
            await _datacenterService.CreateDatacenterAsync(datacenterDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _datacenterService.DeleteDatacenterAsync(id);
            return Ok();
        }

        private async Task<ActionResult> GetChangePlanChassis(List<GetAssetsApiDto> response, Guid changePlanId)
        {
            var changePlanItems = await _changePlanService.GetChangePlanItemsAsync(changePlanId);
            foreach (ChangePlanItemDto changePlanItem in changePlanItems)
            {
                if (changePlanItem.ExecutionType.Equals("decommission"))
                {
                    var decommissionedAssetDto = (JsonConvert.DeserializeObject<DecommissionedAssetDto>(changePlanItem.NewData));
                    var remove = response.Find(x => x.Id == decommissionedAssetDto.Id);
                    response.Remove(remove);
                    continue;
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
                var assetApiDto = _mapper.Map<GetAssetsApiDto>(assetDto);
                if (assetApiDto.MountType.Contains("chassis"))
                {
                    var remove = response.Find(x => x.Id == assetDto.Id);
                    response.Remove(remove);
                    response.Add(assetApiDto);
                }
                
            }
            return Ok();
        }
    }
}