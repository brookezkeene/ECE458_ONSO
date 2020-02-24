using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Web.Api.Common;
using Web.Api.Core;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos;
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

        public DatacentersController(IDatacenterService datacenterService, IApiErrorResources errorResources)
        {
            _datacenterService = datacenterService;
            _errorResources = errorResources;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<GetDatacenterApiDto>>> GetMany(string searchText, int page = 1, int pageSize = 10)
        {
            var datacenters = await _datacenterService.GetDatacentersAsync(searchText, page, pageSize);
            var response = datacenters.MapTo<PagedList<GetDatacenterApiDto>>();
            return Ok(response);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<GetDatacenterApiDto>> Get(Guid id)
        {
            var datacenter = await _datacenterService.GetDatacenterAsync(id);
            var response = datacenter.MapTo<GetDatacenterApiDto>();
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Put(UpdateDatacenterApiDto datacenterApiDto)
        {
            var datacenterDto = datacenterApiDto.MapTo<DatacenterDto>();
            await _datacenterService.UpdateDatacenterAsync(datacenterDto);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Post(CreateDatacenterApiDto datacenterApiDto)
        {
            var datacenterDto = datacenterApiDto.MapTo<DatacenterDto>();
            await _datacenterService.CreateDatacenterAsync(datacenterDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _datacenterService.DeleteDatacenterAsync(id);
            return Ok();
        }
    }
}