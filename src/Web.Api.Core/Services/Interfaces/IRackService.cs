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
        Task<PagedList<FlatRackDto>> GetRacksAsync(string search, int page = 1, int pageSize = 10);
        Task CreateRacksAsync(RackRangeQuery query);
        Task DeleteRacksAsync(RackRangeQuery query);
    }
}
