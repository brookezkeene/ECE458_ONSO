using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web.Api.Common;
using Web.Api.Core;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos;
using Web.Api.Dtos.Assets.Read;
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
        private readonly IMapper _mapper;

        public DatacentersController(IDatacenterService datacenterService, IApiErrorResources errorResources, IMapper mapper)
        {
            _datacenterService = datacenterService;
            _errorResources = errorResources;
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
            var datacenter = await _datacenterService.GetDatacenterAsync(id);
            await _datacenterService.DeleteDatacenterAsync(datacenter);
            return Ok();
        }
    }
}