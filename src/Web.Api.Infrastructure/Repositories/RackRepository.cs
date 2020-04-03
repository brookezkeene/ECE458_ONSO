using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Api.Common;
using Web.Api.Common.Extensions;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Entities.Extensions;
using Web.Api.Infrastructure.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Infrastructure.Repositories
{
    public class RackRepository<TDbContext> : IRackRepository
        where TDbContext : DbContext, IApplicationDbContext
    {
        private readonly TDbContext _dbContext;

        public RackRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<Rack>> GetRacksAsync(Guid? datacenterId, string sortBy, string isDesc, int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<Rack>();
            var racks = await Sort(datacenterId,  sortBy,  isDesc,  page = 1,  pageSize = 10);
            pagedList.AddRange(racks);
            pagedList.TotalCount = await _dbContext.Racks
                .WhereIf(datacenterId != null, x => x.Datacenter.Id == datacenterId)
                .CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.CurrentPage = page;

            return pagedList;
        }

        public async Task<List<Rack>> GetRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd, Guid? datacenterId)
        {
            var racks = await _dbContext.Racks
                //.Include(x => x.Assets)
                //    .ThenInclude(i => i.Model)
                //.Include(x => x.Assets)
                //    .ThenInclude(i => i.Owner)
                .Where(x => x.Column >= colStart && x.Column <= colEnd)
                .WhereIf(datacenterId != null && datacenterId.Value != default, x => x.DatacenterId == datacenterId)
                //.AsNoTracking()
                .ToListAsync();
                racks = racks.Where(x => x.Row[0] >= rowStart[0] && x.Row[0] <= rowEnd[0]).ToList();

            return racks;
        }

        public async Task<Rack> GetRackAsync(Guid rackId)
        {
            return await _dbContext.Racks
                //.Include(o => o.Pdus)
                //.ThenInclude(o => o.Ports)
                .Where(x => x.Id == rackId)
                //.AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<Rack> GetRackAsync(string row, int column, Guid datacenterId)
        {
            return await _dbContext.Racks
                .SingleOrDefaultAsync(x => x.Row == row && x.Column == column && x.DatacenterId == datacenterId);
        }

        public Rack GetRack(string row, int column, Guid datacenterId)
        {
            return _dbContext.Racks
                .SingleOrDefault(rack => rack.Row == row && rack.Column == column && rack.DatacenterId == datacenterId);
        }
        public async Task<int> AddRackAsync(Rack rack)
        {
            _dbContext.Racks.Add(rack);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateRackAsync(Rack rack)
        {
            _dbContext.Racks.Update(rack);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteRackAsync(Rack rack)
        {
            _dbContext.Racks.Remove(rack);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CreateRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd,
            Guid datacenterId)
        {
            var racksToAdd = new List<Rack>();
            for (var r = rowStart[0]; r <= rowEnd[0]; r++)
            {
                var row = r.ToString().ToUpper();
                for (var col = colStart; col <= colEnd; col++)
                {
                    if (!await _dbContext.Racks.AnyAsync(o => o.Row == row && o.Column == col && o.Datacenter.Id == datacenterId))
                    {
                        racksToAdd.Add(new Rack
                        {
                            Column = col, Row = row, DatacenterId = datacenterId,
                            Pdus = (new[] {PduLocation.L, PduLocation.R}).Select(loc => new Pdu
                                {
                                    NumPorts = 24, Location = loc, Ports = Enumerable.Range(1, 24)
                                        .Select(n => new PduPort {Number = n})
                                        .ToList()
                                })
                                .ToList()
                        });
                    }
                }
            }

            await _dbContext.Racks.AddRangeAsync(racksToAdd);

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd,
            Guid datacenterId)
        {
            var racksToRemove = new List<Rack>();
            for (var r = rowStart[0]; r <= rowEnd[0]; r++)
            {
                var row = r.ToString();
                for (var col = colStart; col <= colEnd; col++)
                {
                    var eligibleForDeletion =
                        await _dbContext.Racks
                            .Where(o => o.Row == row && o.Column == col && !o.Assets.Any())
                            .Where(o => o.Datacenter.Id.Equals(datacenterId))
                            .ToListAsync();
                    racksToRemove.AddRange(eligibleForDeletion);
                }
            }

            _dbContext.PduPort.RemoveRange(racksToRemove.SelectMany(o => o.Pdus)
                .SelectMany(o => o.Ports));
            _dbContext.Pdu.RemoveRange(racksToRemove.SelectMany(o => o.Pdus));
            _dbContext.Racks.RemoveRange(racksToRemove);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> AddressExistsAsync(string rackRow, int rackColumn, Guid datacenterId)
        {
            return await _dbContext.Racks.Where(x => x.Row == rackRow && x.Column == rackColumn && x.Datacenter.Id == (datacenterId))
                .AnyAsync();
        }
        private async Task<List<Rack>> Sort(Guid? datacenterId, string sortBy, string isDesc, int page = 1, int pageSize = 10)
        {

            if (!string.IsNullOrEmpty(sortBy) && sortBy.Equals("address"))
            {
                if (isDesc.Equals("false"))
                {
                    return await _dbContext.Racks
                    .WhereIf(datacenterId != null, x => x.DatacenterId == datacenterId)
                    .PageBy(x => x.Row + x.Column.ToString(), page, pageSize, false)
                    .ToListAsync();
                }
                else if (isDesc.Equals("true"))
                {
                    return await _dbContext.Racks
                    .WhereIf(datacenterId != null, x => x.DatacenterId == datacenterId)
                    .PageBy(x => x.Row + x.Column.ToString(), page, pageSize, true)
                    .ToListAsync();
                }
            }
            else if (!string.IsNullOrEmpty(sortBy) && sortBy.Equals("datacenter.description"))
            {
                if (isDesc.Equals("false"))
                {
                    return await _dbContext.Racks
                    .WhereIf(datacenterId != null, x => x.DatacenterId == datacenterId)
                    .PageBy(x => x.Datacenter.Name, page, pageSize, false)
                    .ToListAsync();
                }
                else if (isDesc.Equals("true"))
                {
                    return await _dbContext.Racks
                    .WhereIf(datacenterId != null, x => x.DatacenterId == datacenterId)
                    .PageBy(x => x.Datacenter.Name, page, pageSize, true)
                    .ToListAsync();
                }
            }
            return await _dbContext.Racks
                .WhereIf(datacenterId != null, x => x.DatacenterId == datacenterId)
                .PageBy(x => x.Id, page, pageSize)
                .ToListAsync();
        }
    }
}
