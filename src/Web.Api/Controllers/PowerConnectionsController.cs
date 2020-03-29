using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PowerConnectionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPowerService _powerService;

        public PowerConnectionsController(IMapper mapper, IPowerService powerService)
        {
            _mapper = mapper;
            _powerService = powerService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMany(List<CreatePowerConnectionApiDto> connectionApiDtos)
        {
            var connections = _mapper.Map<List<PowerConnectionDto>>(connectionApiDtos);
            await _powerService.CreateConnectionsAsync(connections);
            return Ok();
        }
    }

    public class CreatePowerConnectionApiDto
    {
        public Guid PduPortId { get; set; }
        public Guid AssetPowerPortId { get; set; }
    }
}