using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Api.Common;
using Web.Api.Common.Extensions;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
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

        public async Task<PagedList<Rack>> GetRacksAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<Rack>();

            var racks = await _dbContext.Racks
                .PageBy(x => x.Id, page, pageSize)
                .AsNoTracking()
                .ToListAsync();

            pagedList.AddRange(racks);
            pagedList.TotalCount = await _dbContext.Racks
                .CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.CurrentPage = page;

            return pagedList;
        }

        public async Task<List<Rack>> GetRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd)
        {
            Func<Rack, bool> searchCondition = x => x.Row.BetweenIgnoreCase(rowStart, rowEnd) && x.Column.Between(colStart, colEnd);

            var racks = await _dbContext.Racks
                .Include(x => x.Instances)
                    .ThenInclude(i => i.Model)
                .Include(x => x.Instances)
                    .ThenInclude(i => i.Owner)
                .Where(x => x.Column >= colStart && x.Column <= colEnd)
                .AsNoTracking()
                .ToListAsync();
                racks = racks.Where(x => x.Row[0] >= rowStart[0] && x.Row[0] <= rowEnd[0]).ToList();

            return racks;
        }

        public async Task<Rack> GetRackAsync(Guid rackId)
        {
            return await _dbContext.Racks
                .Include(x => x.Instances)
                .Where(x => x.Id == rackId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<Rack> GetRackAsync(string row, int col)
        {
            return await _dbContext.Racks
                .Include(x => x.Instances)
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

        public async Task CreateRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd)
        {
            for (var r = rowStart[0]; r <= rowEnd[0]; r++)
            {
                var row = r.ToString();
                for (var col = colStart; col <= colEnd; col++)
                {
                    if (!await _dbContext.Racks.AnyAsync(o => o.Row == row && o.Column == col))
                    { 
                        await _dbContext.Racks.AddAsync(new Rack {Column = col, Row = row});
                    }
                }
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd)
        {
            for (var r = rowStart[0]; r <= rowEnd[0]; r++)
            {
                var row = r.ToString();
                for (var col = colStart; col <= colEnd; col++)
                {
                    var eligibleForDeletion =
                        await _dbContext.Racks.Where(o => o.Row == row && o.Column == col && !o.Instances.Any())
                            .ToListAsync();
                    eligibleForDeletion.ForEach(rack => _dbContext.Remove(rack));
                }
            }
            await _dbContext.SaveChangesAsync();
        }

    }
}
