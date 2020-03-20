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

        public async Task<PagedList<Asset>> GetAssetsAsync(Guid? datacenterId, string vendor, string number, string hostname, string rackStart, string rackEnd,
                    string sortBy, string isDesc, int page, int pageSize)
        {
            var pagedList = new PagedList<Asset>();
            Expression<Func<Asset, bool>> hostnameCondition = x => (x.Hostname.Contains(hostname));
            Expression<Func<Asset, bool>> vendorCondition = x => (x.Model.Vendor.Contains(vendor));
            Expression<Func<Asset, bool>> numberCondition = x => (x.Model.ModelNumber.Contains(number));

            var assets = await _dbContext.Assets
                .Include(x => x.Model)
                .Include(x => x.Owner)
                .Include(x => x.Rack)
                .ThenInclude(x => x.Datacenter)
                .WhereIf(datacenterId != null, x => x.Rack.Datacenter.Id == datacenterId)
                .WhereIf(!string.IsNullOrEmpty(hostname), hostnameCondition)
                .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                .PageBy(x => x.Hostname, page, pageSize)
                .AsNoTracking()
                .ToListAsync();
            assets = assets.Where(x => String.Compare($"{x.Rack.Row.ToUpper()}{x.Rack.Column}", rackStart) >= 0 &&
                                  String.Compare($"{x.Rack.Row.ToUpper()}{x.Rack.Column}", rackEnd) <= 0).ToList();
            assets = Sort(assets, sortBy, isDesc);
            pagedList.AddRange(assets);
            pagedList.TotalCount = await _dbContext.Assets
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
                .Where(x => x.Rack.Column >= colStart)
                .Where(x => x.Rack.Column <= colEnd)
                .WhereIf(!string.IsNullOrEmpty(search), modelCondition)
                .WhereIf(!string.IsNullOrEmpty(hostname), hostnameCondition)
                .AsNoTracking()
                .ToListAsync();
                assets = assets.Where(x => x.Rack.Row[0] >= rowStart[0] && x.Rack.Row[0] <= rowEnd[0]).ToList();
    

            return assets;
        }

        public async Task<List<AssetNetworkPort>> GetNetworkPortExportAsync(string search, string hostname, string rowStart, int colStart, string rowEnd, int colEnd)
        {

            Expression<Func<AssetNetworkPort, bool>> modelCondition = x => x.Asset.Model.ModelNumber.ToUpper().Contains(search) || x.Asset.Model.Vendor.ToUpper().Contains(search);
            Expression<Func<AssetNetworkPort, bool>> hostnameCondition = x => x.Asset.Hostname.ToUpper().Contains(hostname);

            var ports = await _dbContext.AssetNetworkPort
                .Include(x => x.ModelNetworkPort)
                .Include(x => x.Asset).ThenInclude(x => x.Rack).ThenInclude(x => x.Datacenter)
                .Include(x => x.ConnectedPort).ThenInclude(x => x.ModelNetworkPort)
                .Include(x => x.ConnectedPort).ThenInclude(x => x.Asset)
                .WhereIf(!string.IsNullOrEmpty(search), modelCondition)
                .WhereIf(!string.IsNullOrEmpty(hostname), hostnameCondition)
                .AsNoTracking()
                .ToListAsync();
            ports = ports.Where(x => x.Asset.Rack.Row[0] >= rowStart[0] && x.Asset.Rack.Row[0] <= rowEnd[0] && x.Asset.Rack.Column >= colStart && x.Asset.Rack.Column <= colEnd).ToList();

            return ports;
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
                    .ThenInclude(x => x.ConnectedPort)
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

        public async Task<Asset> GetAssetForDecommissioning(Guid assetId)
        {
            return await _dbContext.Assets
                .Include(x => x.Model)
                .Include(x => x.Owner)
                .Include(x => x.Rack)
                    .ThenInclude(x => x.Datacenter)
                .Include(x => x.NetworkPorts)
                    .ThenInclude(x => x.ModelNetworkPort)
                .Include(x => x.NetworkPorts)
                    .ThenInclude(x => x.ConnectedPort)
                        .ThenInclude(x => x.Asset)
                 .Include(x => x.NetworkPorts)
                    .ThenInclude(x => x.ConnectedPort)
                        .ThenInclude(x => x.ModelNetworkPort)
                .Include(x => x.PowerPorts)
                    .ThenInclude(x => x.PduPort)
                        .ThenInclude(x => x.Pdu)
                            .ThenInclude(x => x.Rack)
                                .ThenInclude(x => x.Datacenter)
                .Where(x => x.Id == assetId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }
        public async Task<PagedList<DecommissionedAsset>> GetDecommissionedAssetsAsync( int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<DecommissionedAsset>();

            var assets = await _dbContext.DecommissionedAssets
                .PageBy(x => x.Id, page, pageSize)
                .AsNoTracking()
                .ToListAsync();

            pagedList.AddRange(assets);
            pagedList.TotalCount = await _dbContext.Assets
                .CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.CurrentPage = page;

            return pagedList;
        }
        public async Task<int> AddDecomissionedAssetAsync(DecommissionedAsset asset)
        {
            _dbContext.DecommissionedAssets.Add(asset);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<DecommissionedAsset> GetDecommissionedAssetAsync(Guid assetId)
        {
            return await _dbContext.DecommissionedAssets
                .Where(x => x.Id == assetId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        private static List<Asset> Sort(List<Asset> assets, string sortBy, string isDesc)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return assets;
            }
            else if (sortBy.Equals("vendor"))
            {
                if (isDesc.Equals("false"))
                {
                    return assets.OrderBy(q => q.Model.Vendor).ToList();
                }
                else if (isDesc.Equals("true"))
                {
                    return assets.OrderByDescending(q => q.Model.Vendor).ToList();
                }
            }
            else if (sortBy.Equals("modelNumber"))
            {
                if (isDesc.Equals("false"))
                {
                    return assets.OrderBy(q => q.Model.ModelNumber).ToList();
                }
                else if (isDesc.Equals("true"))
                {
                    return assets.OrderByDescending(q => q.Model.ModelNumber).ToList();
                }
            }
            else if (sortBy.Equals("hostname"))
            {
                if (isDesc.Equals("false"))
                {
                    return assets.OrderBy(q => q.Hostname).ToList();
                }
                else if (isDesc.Equals("true"))
                {
                    return assets.OrderByDescending(q => q.Hostname).ToList();
                }
            }
            else if (sortBy.Equals("datacenter"))
            {
                if (isDesc.Equals("false"))
                {
                    return assets.OrderBy(q => q.Rack.Datacenter.Name).ToList();
                }
                else if (isDesc.Equals("true"))
                {
                    return assets.OrderByDescending(q => q.Rack.Datacenter.Name).ToList();
                }
            }
            else if (sortBy.Equals("rack"))
            {
                if (isDesc.Equals("false"))
                {
                    return assets.OrderBy(c => c.Rack.Row).ThenBy(c => c.Rack.Column).ThenBy(c => c.RackPosition).ToList();
                }
                else if (isDesc.Equals("true"))
                {
                    return assets.OrderByDescending(c => c.Rack.Row).ThenByDescending(c => c.Rack.Column).ThenByDescending(c => c.RackPosition).ToList();
                }
            }
            else if (sortBy.Equals("rackPosition"))
            {
                if (isDesc.Equals("false"))
                {
                    return assets.OrderBy(q => q.RackPosition).ToList();
                }
                else if (isDesc.Equals("true"))
                {
                    return assets.OrderByDescending(q => q.RackPosition).ToList();
                }
            }
            else if (sortBy.Equals("owner"))
            {
                if (isDesc.Equals("false"))
                {
                    return assets.OrderBy(q => q.Owner.UserName).ToList();
                }
                else if (isDesc.Equals("true"))
                {
                    return assets.OrderByDescending(q => q.Owner.UserName).ToList();
                }
            }
            return assets;
        }
    }
}
