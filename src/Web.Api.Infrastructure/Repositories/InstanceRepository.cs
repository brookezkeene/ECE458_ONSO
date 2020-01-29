using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Api.Common;
using Web.Api.Common.Extensions;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Infrastructure.Repositories
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

            pagedList.AddRange(instances);
            pagedList.TotalCount = await _dbContext.Instances
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.CurrentPage = page;

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
