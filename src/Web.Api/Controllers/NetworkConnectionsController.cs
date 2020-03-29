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
using Web.Api.Dtos.Datacenters.Read;
using Web.Api.Mappers;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NetworkConnectionsController : ControllerBase
    {
        private readonly INetworkService _networkService;
        private readonly IMapper _mapper;

        public NetworkConnectionsController(INetworkService networkService, IMapper mapper)
        {
            _networkService = networkService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateMany(List<CreateNetworkConnectionApiDto> connectionApiDtos)
        {
            var networkConnections = _mapper.Map<List<NetworkConnectionDto>>(connectionApiDtos);
            await _networkService.CreateConnectionsAsync(networkConnections);
            return Ok();
        }
    }
}