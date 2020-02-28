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
    public class DatacenterRepository : IDatacenterRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DatacenterRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> AddDatacenterAsync(Datacenter datacenter)
        {
            _dbContext.Datacenters.Add(datacenter);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> CanDeleteDatacenterAsync(Datacenter datacenter) // not sure if this is proper implementation here
        {
            bool racksExist = await RacksInDatacenterExistAsync(datacenter);
            return !racksExist;
        }

        public async Task<bool> DatacenterExistsAsync(string name, string description, Guid id)
        {
            return await _dbContext.Datacenters
                            // lookup by id if we were given one
                            .WhereIf(id != default, x => x.Id == id)
                            // otherwise lookup by vendor & model number
                            .WhereIf(id == default, x => x.Name == name && x.Description == description)
                            .AnyAsync();
        }

        public async Task<bool> DatacenterIsUniqueAsync(string name, string description, Guid id = default)
        {
            return !await _dbContext.Datacenters
                            .Where(x => x.Name == name && x.Description == description)
                            .WhereIf(id != default, x => x.Id != id)
                            .AnyAsync();
        }

        public async Task<List<AssetNetworkPort>> GetNetworkPortFromDatacenterAsync(Guid datacenterID)
        {
            var ports = await _dbContext.AssetNetworkPort
                .Include(x => x.ModelNetworkPort)
                .Include(x => x.Asset).ThenInclude(x => x.Rack)
                .Include(x => x.ConnectedPort).ThenInclude(x => x.ModelNetworkPort)
                .Include(x => x.ConnectedPort).ThenInclude(x => x.Asset)
                .Where(x => x.Asset.Rack.Datacenter.Id == datacenterID)
                .AsNoTracking()
                .ToListAsync();
            return ports;
        }

        public async Task<int> DeleteDatacenterAsync(Datacenter datacenter)
        {
            _dbContext.Datacenters.Remove(datacenter);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<Datacenter> GetDatacenterAsync(Guid datacenterId)
        {
            return await _dbContext.Datacenters
                .Where(x => x.Id == datacenterId)
                .AsNoTracking()
                .SingleAsync();
        }

        public async Task<PagedList<Datacenter>> GetDatacentersAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<Datacenter>();
            //if 
            Expression<Func<Datacenter, bool>> searchCondition = x => (x.Name.Contains(search) || x.Description.Contains(search));

            var datacenters = await _dbContext.Datacenters
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .PageBy(x => x.Name, page, pageSize)
                .AsNoTracking()
                .ToListAsync();

            pagedList.AddRange(datacenters);
            pagedList.TotalCount = await _dbContext.Datacenters
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.CurrentPage = page;

            return pagedList;
        }

        public async Task<bool> RacksInDatacenterExistAsync(Datacenter datacenter) // TODO: add datacenters to racks
        {
            return await _dbContext.Racks.Where(x => x.Datacenter == datacenter)
                            .AnyAsync();
        }

        public async Task<int> UpdateDatacenterAsync(Datacenter datacenter)
        {
            _dbContext.Datacenters.Update(datacenter);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
