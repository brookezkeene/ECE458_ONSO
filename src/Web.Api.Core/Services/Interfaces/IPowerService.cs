using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Dtos;
using Web.Api.Core.Dtos.Power;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IPowerService
    {
        public Task<AssetPowerStateDto> setStates(Guid assetId, PowerState state);

        public Task<AssetPowerStateDto> getStates(Guid assetId);

    }
}
