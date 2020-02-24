using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos;
using Web.Api.Dtos.Bulk.Export;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IModelService _modelService;
        private readonly IAssetService _assetService;

        public ExportController(IModelService modelService, IAssetService assetService)
        {
            _modelService = modelService;
            _assetService = assetService;
        }

        [HttpGet("models")]
        public async Task<ActionResult<List<ExportModelDto>>> Get([FromQuery] ModelExportQuery query)
        {
            var models = await _modelService.GetModelExportAsync(query);
            return Ok(models);
        }

        [HttpGet("assets")]
        public async Task<ActionResult<List<ExportModelDto>>> Get([FromQuery] AssetExportQuery query)
        {
            var assets = await _assetService.GetAssetExportAsync(query);
            return Ok(assets);
        }

    }
}