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

        [HttpGet("range")]
        public async Task<ActionResult<List<FlatModelDto>>> Get([FromQuery] ExportQuery query)
        {
            var models = await _modelService.GetModelExportAsync(query);
            return Ok(models);
        }

    }
}