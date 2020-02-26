using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic.CompilerServices;
using Web.Api.Common;
using Web.Api.Common.Extensions;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Entities.Extensions;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Infrastructure.Repositories
{
    public class RackRepository : IRackRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RackRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<Rack>> GetRacksAsync(Guid? datacenterId, int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<Rack>();

            var query = _dbContext.Racks
                .PageBy(x => x.Id, page, pageSize)
                .WhereIf(datacenterId != null, x => x.DatacenterId == datacenterId)
                .Include(x => x.Datacenter)
                .AsNoTracking();
            var racks = await query.ToListAsync();

            pagedList.AddRange(racks);
            pagedList.TotalCount = await query.CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.CurrentPage = page;

            return pagedList;
        }

        public async Task<List<Rack>> GetRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd, Guid? datacenterId)
        {
            Func<Rack, bool> searchCondition = x => x.Row.BetweenIgnoreCase(rowStart, rowEnd) && x.Column.Between(colStart, colEnd);

            var racks = await _dbContext.Racks
                .Include(x => x.Assets)
                    .ThenInclude(i => i.Model)
                .Include(x => x.Assets)
                    .ThenInclude(i => i.Owner)
                .Where(x => x.Column >= colStart && x.Column <= colEnd)
                .WhereIf(datacenterId != null, x => x.DatacenterId == datacenterId)
                .AsNoTracking()
                .ToListAsync();
                racks = racks.Where(x => x.Row[0] >= rowStart[0] && x.Row[0] <= rowEnd[0]).ToList();

            return racks;
        }

        public async Task<Rack> GetRackAsync(Guid rackId)
        {
            return await _dbContext.Racks
                .Include(o => o.Pdus)
                .ThenInclude(o => o.Ports)
                .Where(x => x.Id == rackId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<Rack> GetRackAsync(string row, int col)
        {
            return await _dbContext.Racks
                .Include(x => x.Assets)
                .Where(x => x.Row == row && x.Column == col)
                .AsNoTracking()
                .SingleOrDefaultAsync();
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
            for (var r = rowStart[0]; r <= rowEnd[0]; r++)
            {
                var row = r.ToString().ToUpper();
                for (var col = colStart; col <= colEnd; col++)
                {
                    if (!await _dbContext.Racks.AnyAsync(o => o.Row == row && o.Column == col))
                    {
                        await _dbContext.Racks.AddAsync(new Rack
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

            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd,
            Guid datacenterId)
        {
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
                    eligibleForDeletion.ForEach(rack => _dbContext.Remove(rack));
                }
            }
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> AddressExistsAsync(string rackRow, int rackColumn, Guid datacenterId)
        {
            return await _dbContext.Racks.Where(x => x.Row == rackRow && x.Column == rackColumn && x.Datacenter.Id == (datacenterId))
                .AnyAsync();
        }
    }
}
