using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IAssetRepository
    {
        Task<PagedList<Asset>> GetAssetsAsync(string search, int page = 1, int pageSize = 10);
        Task<Asset> GetAssetAsync(Guid assetId);
        Task<int> AddAssetAsync(Asset asset);
        Task<int> UpdateAssetAsync(Asset asset);
        Task<int> DeleteAssetAsync(Asset asset);
        Task<List<Asset>> GetAssetExportAsync(string search, string hostname, string rowStart, int colStart, string rowEnd, int colEnd);
        Task<bool> AssetIsUniqueAsync(string hostname, Guid id = default);
        Task<AssetNetworkPort> GetConnectedNetworkPortAsync(Guid? networkId);
        Task<int> UpdateConnectedNetworkPortAsync(AssetNetworkPort port);
        Task<List<AssetNetworkPort>> GetNetworkPortExportAsync(string search, string hostname, string rowStart, int colStart, string rowEnd, int colEnd);
    }
}
