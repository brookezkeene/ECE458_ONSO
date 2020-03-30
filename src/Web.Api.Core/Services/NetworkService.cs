using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    public class NetworkService : INetworkService
    {
        private readonly INetworkRepository _repository;
        private readonly IMapper _mapper;

        public NetworkService(INetworkRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Guid> CreateConnectionAsync(NetworkConnectionDto connectionDto)
        {
            var connection = _mapper.Map<NetworkConnection>(connectionDto);
            return await _repository.AddConnectionAsync(connection);
        }

        public async Task<NetworkConnectionDto> GetConnectionAsync(Guid connectionId)
        {
            var connection = await _repository.GetConnectionAsync(connectionId);
            return _mapper.Map<NetworkConnectionDto>(connection);
        }

        public async Task<List<Guid>> CreateConnectionsAsync(List<NetworkConnectionDto> connectionDtos)
        {
            var connections = _mapper.Map<List<NetworkConnection>>(connectionDtos);
            return await _repository.AddConnectionsAsync(connections);
        }

        public AssetNetworkPortDto GetNetworkPort(string hostname, string portName)
        {
            var port = _repository.GetNetworkPort(hostname, portName);
            return _mapper.Map<AssetNetworkPortDto>(port);
        }
    }
}