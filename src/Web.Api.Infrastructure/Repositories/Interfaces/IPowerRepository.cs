using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IPowerRepository
    {
        Task<Guid> CreateConnectionAsync(PowerConnection connection);
        Task<PowerConnection> GetConnectionAsync(Guid connectionId);
        AssetPowerPort GetAssetPowerPort(Guid assetPowerPortId);
        PduPort GetPduPort(Guid pduPortId);
        PduPort GetPduPort(Guid rackId, PduLocation pduLocation, int portNumber);
        List<AssetPowerPort> GetAssetPowerPorts(Guid assetId);
        Task<List<Guid>> CreateConnectionsAsync(List<PowerConnection> connections);
    }
}