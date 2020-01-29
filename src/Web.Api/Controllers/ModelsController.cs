using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common;
using Web.Api.Core;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ModelsController : ControllerBase
    {
        private IModelService _modelService;

        public ModelsController(IModelService modelService)
        {
            _modelService = modelService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<ModelDto>>> Get(string searchText, int page = 1, int pageSize = 10)
        {
            var users = await _modelService.GetModelsAsync(searchText, page, pageSize);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ModelDto>> Get(Guid id)
        {
            var user = await _modelService.GetModelAsync(id);

            return Ok(user);
        }
    }

    public class ModelsApiDto
    {
    }

    public class ModelApiDto
    {
    }
}