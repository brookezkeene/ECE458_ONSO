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
    public class AssetRepository : IAssetRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AssetRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<Asset>> GetAssetsAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<Asset>();
            Expression<Func<Asset, bool>> searchCondition = x => x.Hostname.Contains(search);

            var assets = await _dbContext.Assets
                .Include(x => x.Model)
                .Include(x => x.Owner)
                .Include(x => x.Rack)
                .ThenInclude(x => x.Datacenter)
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .PageBy(x => x.Hostname, page, pageSize)
                .AsNoTracking()
                .ToListAsync();

            pagedList.AddRange(assets);
            pagedList.TotalCount = await _dbContext.Assets
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.CurrentPage = page;

            return pagedList;
        }

        public async Task<List<Asset>> GetAssetExportAsync(string search, string hostname, string rowStart, int colStart, string rowEnd, int colEnd)
        {

            Expression<Func<Asset, bool>> modelCondition = x => x.Model.ModelNumber.ToUpper().Contains(search) || x.Model.Vendor.ToUpper().Contains(search);
            Expression<Func<Asset, bool>> hostnameCondition = x => x.Hostname.ToUpper().Contains(hostname);
            

            var assets = await _dbContext.Assets
                .Include(x => x.Model)
                .Include(x => x.Owner)
                .Include(x => x.Rack)
                .Where(x => x.Rack.Row[0] >= rowStart[0] && x.Rack.Column >= colStart && x.Rack.Row[0] <= rowEnd[0] && x.Rack.Column <= colEnd)
                .WhereIf(!string.IsNullOrEmpty(search), modelCondition)
                .WhereIf(!string.IsNullOrEmpty(hostname), hostnameCondition)
                .AsNoTracking()
                .ToListAsync();


            return assets;
        }

        public async Task<Asset> GetAssetAsync(Guid assetId)
        {
            return await _dbContext.Assets
                .Include(x => x.Model)
                .Include(x => x.Owner)
                .Include(x => x.Rack)
                    .ThenInclude(x => x.Datacenter)
                .Include(x => x.PowerPorts)
                    .ThenInclude(x => x.PduPort)
                        .ThenInclude(x => x.Pdu)
                            .ThenInclude(x => x.Rack)
                                .ThenInclude(x => x.Datacenter)
                .Include(x => x.NetworkPorts)
                .Where(x => x.Id == assetId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<int> AddAssetAsync(Asset asset)
        {
            _dbContext.Assets.Add(asset);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateAssetAsync(Asset asset)
        {
            _dbContext.Assets.Update(asset);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAssetAsync(Asset asset)
        {

            _dbContext.Assets.Remove(asset);
            return await _dbContext.SaveChangesAsync();

        }

        public async Task<bool> AssetIsUniqueAsync(string hostname, Guid id = default)
        {
            return !await _dbContext.Assets
                .Where(x => x.Hostname == hostname)
                .WhereIf(id != default, x => x.Id != id)
                .AnyAsync();
        }
    }
}
