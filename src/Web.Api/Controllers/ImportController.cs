using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services;
using Web.Api.Core.Services.Interfaces;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IModelImportService _modelImportService;
        private readonly IAssetImportService _assetImportService;
        private readonly INetworkConnectionImportService _networkConnectionImportService;

        public ImportController(IModelImportService modelImportService, IAssetImportService assetImportService, INetworkConnectionImportService networkConnectionImportService)
        {
            _modelImportService = modelImportService;
            _assetImportService = assetImportService;
            _networkConnectionImportService = networkConnectionImportService;
        }

        [HttpPost("models")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<ImportValidationDto>> UploadModels()
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null || file.Length <= 0) return BadRequest();

            await using var stream = file.OpenReadStream();
            var result = await _modelImportService.HandleImport(stream);

            return Ok(result);
        }

        [HttpGet("models/{id}")]
        public async Task<ActionResult<BulkImportResultDto>> FinalizeModelImport(Guid id)
        {
            var result = await _modelImportService.FinalizeImport(id);
            return Ok(result);
        }

        [HttpPost("assets")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<ImportValidationDto>> UploadAssets()
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null || file.Length <= 0) return BadRequest();

            await using var stream = file.OpenReadStream();
            var result = await _assetImportService.HandleImport(stream);

            return Ok(result);
        }

        [HttpGet("assets/{id}")]
        public async Task<ActionResult<BulkImportResultDto>> FinalizeAssetImport(Guid id)
        {
            var result = await _assetImportService.FinalizeImport(id);
            return Ok(result);
        }

        [HttpPost("networkConnections")]
        [DisableRequestSizeLimit]
        public async Task<ActionResult<ImportValidationDto>> UploadNetworkConnections()
        {
            var file = Request.Form.Files.FirstOrDefault();
            if (file == null || file.Length <= 0) return BadRequest();

            await using var stream = file.OpenReadStream();
            var result = await _networkConnectionImportService.HandleImport(stream);

            return Ok(result);
        }

        [HttpGet("networkConnections/{id}")]
        public async Task<ActionResult<BulkImportResultDto>> UploadNetworkConnections(Guid id)
        {
            var result = await _networkConnectionImportService.FinalizeImport(id);
            return Ok(result);
        }
    }
}