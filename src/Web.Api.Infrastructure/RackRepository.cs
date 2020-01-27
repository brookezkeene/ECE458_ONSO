using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Api.Common;

namespace Web.Api.Infrastructure
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
            throw new NotImplementedException();
        }

        public async Task<List<Rack>> GetRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd)
        {
            Expression<Func<Rack, bool>> searchCondition = x => x.Row.BetweenIgnoreCase(rowStart, rowEnd) && x.Column.Between(colStart, colEnd);

            var racks = await _dbContext.Racks
                .Include(x => x.Instances)
                .Where(searchCondition)
                .AsNoTracking()
                .ToListAsync();

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
    }
}
