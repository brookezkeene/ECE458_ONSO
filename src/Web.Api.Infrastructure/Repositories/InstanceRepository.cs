using System;
using System.Collections.Generic;
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
                .Include(x => x.Model)
                .Include(x => x.Owner)
                .Include(x => x.Rack)
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

        public async Task<List<Instance>> GetInstanceExportAsync(string search, string hostname, string rowStart, int colStart, string rowEnd, int colEnd)
        {

            Expression<Func<Instance, bool>> modelCondition = x => x.Model.ModelNumber.ToUpper().Contains(search) || x.Model.Vendor.ToUpper().Contains(search);
            Expression<Func<Instance, bool>> hostnameCondition = x => x.Hostname.ToUpper().Contains(hostname);
            

            var instances = await _dbContext.Instances
                .Include(x => x.Model)
                .Include(x => x.Owner)
                .Include(x => x.Rack)
                .Where(x => x.Rack.Row[0] >= rowStart[0] && x.Rack.Column >= colStart && x.Rack.Row[0] <= rowEnd[0] && x.Rack.Column <= colEnd)
                .WhereIf(!string.IsNullOrEmpty(search), modelCondition)
                .WhereIf(!string.IsNullOrEmpty(hostname), hostnameCondition)
                .AsNoTracking()
                .ToListAsync();


            return instances;
        }

        public async Task<Instance> GetInstanceAsync(Guid instanceId)
        {
            return await _dbContext.Instances
                .Include(x => x.Model)
                .Include(x => x.Owner)
                .Include(x => x.Rack)
                .Where(x => x.Id == instanceId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<int> AddInstanceAsync(Instance instance)
        {
            var model = instance.Model;
            _dbContext.Entry(model).State = EntityState.Unchanged;

            if (instance.Owner != null)
            {
                var owner = instance.Owner;
                _dbContext.Entry(owner).State = EntityState.Unchanged;
            }
            var rack = instance.Rack;
            _dbContext.Entry(rack).State = EntityState.Unchanged;

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
