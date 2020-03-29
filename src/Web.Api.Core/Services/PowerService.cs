using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Services.Interfaces;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Core.Dtos.Power;
using Newtonsoft.Json;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    public class PowerService : IPowerService
    {
        private readonly IMapper _mapper;
        private readonly IPowerRepository _repository;


        public PowerService(IMapper mapper, IPowerRepository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<Guid> CreateConnectionAsync(PowerConnectionDto connectionDto)
        {
            var connection = _mapper.Map<PowerConnection>(connectionDto);
            return await _repository.CreateConnectionAsync(connection);
        }

        public async Task<List<Guid>> CreateConnectionsAsync(List<PowerConnectionDto> connectionDtos)
        {
            var connections = _mapper.Map<List<PowerConnection>>(connectionDtos);
            return await _repository.CreateConnectionsAsync(connections);
        }

        public async Task<PowerConnectionDto> GetConnectionAsync(Guid connectionId)
        {
            var connection = await _repository.GetConnectionAsync(connectionId);
            return _mapper.Map<PowerConnectionDto>(connection);
        }

        public List<AssetPowerPortDto> GetAssetPowerPorts(Guid assetId)
        {
            var ports = _repository.GetAssetPowerPorts(assetId);
            return _mapper.Map<List<AssetPowerPortDto>>(ports);
        }

        public PduPortDto GetPduPort(Guid rackId, PduLocation pduLocation, int portNumber)
        {
            var port = _repository.GetPduPort(rackId, pduLocation, portNumber);
            return _mapper.Map<PduPortDto>(port);
        }
    }
}
