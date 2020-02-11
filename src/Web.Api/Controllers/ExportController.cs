using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IModelService _modelService;
        private readonly IInstanceService _instanceService;

        public ExportController(IModelService modelService, IInstanceService instanceService)
        {
            _modelService = modelService;
            _instanceService = instanceService;
        }

        [HttpGet("model")]
        public async Task<ActionResult<List<ExportModelDto>>> Get([FromQuery] ModelExportQuery query)
        {
            var models = await _modelService.GetModelExportAsync(query);
            return Ok(models);
        }
        [HttpGet("instance")]
        public async Task<ActionResult<List<ExportModelDto>>> Get([FromQuery] InstanceExportQuery query)
        {
            var instances = await _instanceService.GetInstanceExportAsync(query);
            return Ok(instances);
        }

    }
}