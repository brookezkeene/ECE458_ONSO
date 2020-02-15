using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IRackRepository
    {
        Task<PagedList<Rack>> GetRacksAsync(string search, int page = 1, int pageSize = 10);
        Task<List<Rack>> GetRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd);
        Task<Rack> GetRackAsync(string row, int col);
        Task<Rack> GetRackAsync(Guid rackId);
        Task<int> AddRackAsync(Rack rack);
        Task<int> UpdateRackAsync(Rack rack);
        Task<int> DeleteRackAsync(Rack rack);
        Task CreateRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd);
        Task DeleteRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd);
        Task<bool> AddressExistsAsync(string rackRow, int rackColumn);
    }
}