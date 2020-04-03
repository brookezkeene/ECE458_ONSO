using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Web.Api.Common;
using Web.Api.Common.Extensions;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Extensions;
using Web.Api.Infrastructure.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Infrastructure.Repositories
{
    public class AssetRepository<TDbContext> : IAssetRepository
        where TDbContext : DbContext, IApplicationDbContext
    {
        private readonly TDbContext _dbContext;

        public AssetRepository(TDbContext dbContext)
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
                .Include(asset => asset.Rack)
                .ThenInclude(rack => rack.Pdus)
                .ThenInclude(pdu => pdu.Ports)
                .WhereIf(datacenterId != null, x => x.Rack.Datacenter.Id == datacenterId)
                .WhereIf(!string.IsNullOrEmpty(hostname), hostnameCondition)
                .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                .PageBy(x => x.Hostname, page, pageSize)
                .ToListAsync();
            assets = assets.Where(x => String.Compare(x.Rack.Row.ToUpper(), rackStart[0].ToString()) >= 0 &&
                            String.Compare(x.Rack.Row.ToUpper(), rackEnd[0].ToString()) <= 0 &&
                            x.Rack.Column >= int.Parse(rackStart.Substring(1)) &&
                            x.Rack.Column <= int.Parse(rackEnd.Substring(1)))
                .ToList();
            assets = Sort(assets, sortBy, isDesc);
            pagedList.AddRange(assets);
            pagedList.TotalCount = assets.Count();
            pagedList.PageSize = pageSize;
            pagedList.CurrentPage = page;

            return pagedList;
        }


        public async Task<List<Asset>> GetAssetExportAsync(string search, string hostname, string rowStart, int colStart, string rowEnd, int colEnd)
        {

            Expression<Func<Asset, bool>> modelCondition = x => x.Model.ModelNumber.ToUpper().Contains(search) || x.Model.Vendor.ToUpper().Contains(search);
            Expression<Func<Asset, bool>> hostnameCondition = x => x.Hostname.ToUpper().Contains(hostname);
            

            var assets = await _dbContext.Assets
                .Where(x => x.Rack.Column >= colStart)
                .Where(x => x.Rack.Column <= colEnd)
                .WhereIf(!string.IsNullOrEmpty(search), modelCondition)
                .WhereIf(!string.IsNullOrEmpty(hostname), hostnameCondition)
                .ToListAsync();
                assets = assets.Where(x => x.Rack.Row[0] >= rowStart[0] && x.Rack.Row[0] <= rowEnd[0]).ToList();
    

            return assets;
        }

        public async Task<List<AssetNetworkPort>> GetNetworkPortExportAsync(string search, string hostname, string rowStart, int colStart, string rowEnd, int colEnd)
        {

            Expression<Func<AssetNetworkPort, bool>> modelCondition = x => x.Asset.Model.ModelNumber.ToUpper().Contains(search) || x.Asset.Model.Vendor.ToUpper().Contains(search);
            Expression<Func<AssetNetworkPort, bool>> hostnameCondition = x => x.Asset.Hostname.ToUpper().Contains(hostname);

            var ports = await _dbContext.AssetNetworkPort
                .WhereIf(!string.IsNullOrEmpty(search), modelCondition)
                .WhereIf(!string.IsNullOrEmpty(hostname), hostnameCondition)
                .ToListAsync();
            ports = ports.Where(x => x.Asset.Rack.Row[0] >= rowStart[0] && x.Asset.Rack.Row[0] <= rowEnd[0] && x.Asset.Rack.Column >= colStart && x.Asset.Rack.Column <= colEnd).ToList();

            return ports;
        }

        public async Task<AssetNetworkPort> GetNetworkPortAsync(Guid id)
        {
            return await _dbContext.AssetNetworkPort
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();      
        }
        public async Task<PduPort> GetPowerPortAsync(Guid id)
        {
            return await _dbContext.PduPort
                .Where(x => x.Id == id)
                .SingleOrDefaultAsync();
        }
        public async Task<Asset> GetAssetAsync(Guid assetId)
        {
            return await _dbContext.Assets
                .Include(asset => asset.Rack)
                .ThenInclude(rack => rack.Pdus)
                .ThenInclude(pdu => pdu.Ports)
                .Include(asset => asset.Rack)
                .ThenInclude(rack => rack.Pdus)
                .ThenInclude(pdu => pdu.Ports)
                .SingleOrDefaultAsync(x => x.Id == assetId);
        }

        public async Task<int> AddAssetAsync(Asset asset)
        {
            _dbContext.Assets.Add(asset);
            var added = await _dbContext.SaveChangesAsync();

            _dbContext.DeletePreviousNetworkConnections();
            _dbContext.DeletePreviousPowerConnections();

            return added;
        }

        public async Task<int> UpdateAssetAsync(Asset asset)
        {
            _dbContext.Assets.Update(asset);
            var updated = await _dbContext.SaveChangesAsync();

            _dbContext.DeletePreviousNetworkConnections();
            _dbContext.DeletePreviousPowerConnections();

            return updated;
        }

        public async Task<int> DeleteAssetAsync(Asset asset)
        { 
            _dbContext.Assets.Remove(asset);
            var deleted = await _dbContext.SaveChangesAsync();

            _dbContext.DeletePreviousNetworkConnections();
            _dbContext.DeletePreviousPowerConnections();

            return deleted;
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
                .SingleOrDefaultAsync(x => x.Id == assetId);
        }
        public async Task<PagedList<DecommissionedAsset>> GetDecommissionedAssetsAsync(string datacenterName, string generalSearch, string decommissioner,
                    string dateStart, string dateEnd, string rackStart, string rackEnd, string sortBy, string isDesc, int page, int pageSize)
        {
            var pagedList = new PagedList<DecommissionedAsset>();
            Expression<Func<DecommissionedAsset, bool>> hostnameCondition = x => (x.Hostname.Contains(generalSearch) || 
                                           x.ModelName.Contains(generalSearch) || x.ModelNumber.Contains(generalSearch));
            Expression<Func<DecommissionedAsset, bool>> decommissionerCondition = x => (x.Decommissioner.Contains(decommissioner));
            Expression<Func<DecommissionedAsset, bool>> startDateCondition = x => (x.DateDecommissioned >= DateTime.Parse(dateStart));
            Expression<Func<DecommissionedAsset, bool>> endDateCondition = x => (x.DateDecommissioned <= DateTime.Parse(dateEnd));
            Expression<Func<DecommissionedAsset, bool>> datacenterCondition = x => (x.Datacenter.Contains(datacenterName));

            var assets = await _dbContext.DecommissionedAssets
                .WhereIf(!string.IsNullOrEmpty(generalSearch), hostnameCondition)
                .WhereIf(!string.IsNullOrEmpty(decommissioner), decommissionerCondition)
                .WhereIf(!string.IsNullOrEmpty(dateStart), startDateCondition)
                .WhereIf(!string.IsNullOrEmpty(dateEnd), endDateCondition)
                .WhereIf(!string.IsNullOrEmpty(datacenterName), datacenterCondition)
                .PageBy(x => x.Id, page, pageSize)
                .ToListAsync();
            assets = assets.Where(x => String.Compare(x.Rack[0].ToString().ToUpper(), rackStart[0].ToString()) >= 0 &&
                            String.Compare(x.Rack[0].ToString().ToUpper(), rackEnd[0].ToString()) <= 0 &&
                            int.Parse(x.Rack.Substring(1)) >= int.Parse(rackStart.Substring(1)) &&
                            int.Parse(x.Rack.Substring(1)) <= int.Parse(rackEnd.Substring(1)))
                .ToList();
            assets = SortDecommissionedAsset(assets, sortBy, isDesc);
            pagedList.AddRange(assets);
            pagedList.TotalCount = assets.Count();
            pagedList.PageSize = pageSize;
            pagedList.CurrentPage = page;

            return pagedList;
        }

        public Asset GetAsset(int assetNumber)
        {
            return _dbContext.Assets.SingleOrDefault(asset => asset.AssetNumber == assetNumber);
        }

        public async Task<int> AddDecomissionedAssetAsync(DecommissionedAsset asset)
        {
            _dbContext.DecommissionedAssets.Add(asset);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<DecommissionedAsset> GetDecommissionedAssetAsync(Guid assetId)
        {
            return await _dbContext.DecommissionedAssets
                .SingleOrDefaultAsync(x => x.Id == assetId);
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
        private static List<DecommissionedAsset> SortDecommissionedAsset(List<DecommissionedAsset> assets, string sortBy, string isDesc)
        {
            if (string.IsNullOrEmpty(sortBy))
            {
                return assets;
            }
            else if (sortBy.Equals("vendor"))
            {
                if (isDesc.Equals("false"))
                {
                    return assets.OrderBy(q => q.ModelName).ToList();
                }
                else if (isDesc.Equals("true"))
                {
                    return assets.OrderByDescending(q => q.ModelName).ToList();
                }
            }
            else if (sortBy.Equals("modelNumber"))
            {
                if (isDesc.Equals("false"))
                {
                    return assets.OrderBy(q => q.ModelNumber).ToList();
                }
                else if (isDesc.Equals("true"))
                {
                    return assets.OrderByDescending(q => q.ModelNumber).ToList();
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
                    return assets.OrderBy(q => q.Datacenter).ToList();
                }
                else if (isDesc.Equals("true"))
                {
                    return assets.OrderByDescending(q => q.Datacenter).ToList();
                }
            }
            else if (sortBy.Equals("rackAddress"))
            {
                if (isDesc.Equals("false"))
                {
                    return assets.OrderBy(c => c.Rack[0]).ThenBy(c => int.Parse(c.Rack.Substring(1))).ThenBy(c => c.RackPosition).ToList();
                }
                else if (isDesc.Equals("true"))
                {
                    return assets.OrderByDescending(c => c.Rack[0]).ThenByDescending(c => int.Parse(c.Rack.Substring(1))).ThenByDescending(c => c.RackPosition).ToList();
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
                    return assets.OrderBy(q => q.OwnerName).ToList();
                }
                else if (isDesc.Equals("true"))
                {
                    return assets.OrderByDescending(q => q.OwnerName).ToList();
                }
            }
            else if (sortBy.Equals("dateDecommissioned"))
            {
                if (isDesc.Equals("false"))
                {
                    return assets.OrderBy(q => q.DateDecommissioned).ToList();
                }
                else if (isDesc.Equals("true"))
                {
                    return assets.OrderByDescending(q => q.DateDecommissioned).ToList();
                }
            }
            else if (sortBy.Equals("decommissioner"))
            {
                if (isDesc.Equals("false"))
                {
                    return assets.OrderBy(q => q.Decommissioner).ToList();
                }
                else if (isDesc.Equals("true"))
                {
                    return assets.OrderByDescending(q => q.Decommissioner).ToList();
                }
            }
            return assets;
        }
    }
}
