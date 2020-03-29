using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface INetworkRepository
    {
        Task<Guid> AddConnectionAsync(NetworkConnection connection);
        AssetNetworkPort GetNetworkPort(Guid portId);
        Task<NetworkConnection> GetConnectionAsync(Guid connectionId);
        Task<List<Guid>> AddConnectionsAsync(List<NetworkConnection> connections);
    }
}