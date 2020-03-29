using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Dtos;
using Web.Api.Core.Dtos.Power;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IPowerService
    {
        Task<Guid> CreateConnectionAsync(PowerConnectionDto connectionDto);
        Task<List<Guid>> CreateConnectionsAsync(List<PowerConnectionDto> connectionDtos);
        Task<PowerConnectionDto> GetConnectionAsync(Guid connectionId);
        List<AssetPowerPortDto> GetAssetPowerPorts(Guid assetId);
        PduPortDto GetPduPort(Guid rackId, PduLocation pduLocation, int portNumber);
    }
}
