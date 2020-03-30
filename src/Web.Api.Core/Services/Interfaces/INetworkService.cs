using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Services.Interfaces
{
    public interface INetworkService
    {
        Task<Guid> CreateConnectionAsync(NetworkConnectionDto connectionDto);
        Task<NetworkConnectionDto> GetConnectionAsync(Guid connectionId);
        Task<List<Guid>> CreateConnectionsAsync(List<NetworkConnectionDto> connectionDtos);
        AssetNetworkPortDto GetNetworkPort(string hostname, string portName);
    }
}