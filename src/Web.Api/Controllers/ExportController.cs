using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos;
using Web.Api.Dtos.Bulk.Export;
using Web.Api.Dtos.Models.Read;
using Web.Api.Mappers;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExportController : ControllerBase
    {
        private readonly IModelService _modelService;
        private readonly IAssetService _assetService;
        private readonly IMapper _mapper;

        public ExportController(IModelService modelService, IAssetService assetService, IMapper mapper)
        {
            _modelService = modelService;
            _assetService = assetService;
            _mapper = mapper;
        }

        [HttpGet("models")]
        public async Task<ActionResult<List<ExportModelDto>>> Get([FromQuery] ModelExportQuery query)
        {
            var models = await _modelService.GetModelExportAsync(query);
            var response = new List<ExportModelDto>();
            if (models.Count() != 0)
            {
                foreach (ModelDto model in models)
                {
                    response.Add(_mapper.Map<ExportModelDto>(model));
                }
            }
            return Ok(response);
        }

        [HttpGet("assets")]
        public async Task<ActionResult<List<ExportAssetDto>>> Get([FromQuery] AssetExportQuery query)
        {
            var assets = await _assetService.GetAssetExportAsync(query);
            var response = new List<ExportAssetDto>();
            if (assets.Count() != 0)
            {
                foreach (AssetDto asset in assets)
                {
                    response.Add(_mapper.Map<ExportAssetDto>(asset));
                }
            }
            return Ok(response);
        }


        [HttpGet("networkports")]
        public async Task<ActionResult<List<ExportNetworkPortDto>>> Get([FromQuery] NetworkPortExportQuery query)
        {
            var ports = await _assetService.GetNetworkPortExportAsync(query);
            var response = new List<ExportNetworkPortDto>();

            foreach (AssetNetworkPortDto port in ports)
            {
                var export = _mapper.Map<ExportNetworkPortDto>(port);
                response.Add(export);
            }
            
            return Ok(response);
        }
    }
}