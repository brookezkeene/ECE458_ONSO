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

        public ImportController(IModelImportService modelImportService)
        {
            _modelImportService = modelImportService;
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
        public async Task<ActionResult<BulkImportResultDto>> FinalizeImport(Guid id)
        {
            var result = await _modelImportService.FinalizeImport(id);
            return Ok(result);
        }
    }
}