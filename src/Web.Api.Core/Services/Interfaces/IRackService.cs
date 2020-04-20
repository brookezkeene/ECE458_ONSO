using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IRackService
    {
        Task<List<RackDto>> GetRacksAsync(RackRangeQuery query);
        Task<PagedList<RackDto>> GetRacksAsync(SearchRackQuery query);
        Task CreateRacksAsync(RackRangeQuery query);
        Task DeleteRacksAsync(RackRangeQuery query);
        Task<RackDto> GetAvailablePowerPorts(Guid id);
        Task<RackDto> GetRackDtoAsync(Guid id);
        Task<RackDto> GetOfflineRack(Guid datacenterId);
    }
}
