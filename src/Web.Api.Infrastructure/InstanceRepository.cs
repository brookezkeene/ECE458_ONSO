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
    public class InstanceRepository : IInstanceRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public InstanceRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<Instance>> GetInstancesAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<Instance>();
            Expression<Func<Instance, bool>> searchCondition = x => x.Hostname.Contains(search);

            var instances = await _dbContext.Instances
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .PageBy(x => x.Hostname, page, pageSize)
                .AsNoTracking()
                .ToListAsync();

            pagedList.Data.AddRange(instances);
            pagedList.TotalCount = await _dbContext.Instances
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .CountAsync();
            pagedList.PageSize = pageSize;

            return pagedList;
        }

        public async Task<Instance> GetInstanceAsync(Guid instanceId)
        {
            return await _dbContext.Instances
                .Include(x => x.Model)
                .Where(x => x.Id == instanceId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<int> AddInstanceAsync(Instance instance)
        {
            _dbContext.Instances.Add(instance);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateInstanceAsync(Instance instance)
        {
            _dbContext.Instances.Update(instance);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteInstanceAsync(Instance instance)
        {
            _dbContext.Instances.Remove(instance);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
