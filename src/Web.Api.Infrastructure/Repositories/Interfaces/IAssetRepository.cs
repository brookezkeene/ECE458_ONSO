using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IAssetRepository
    {
        Task<PagedList<Asset>> GetAssetsAsync(Guid? datacenterId, string vendor, string number, string hostname, string rackStart, string rackEnd,
                            string sortBy, string isDesc, int page, int pageSize);
        Task<Asset> GetAssetAsync(Guid assetId);
        Task<int> AddAssetAsync(Asset asset);
        Task<int> UpdateAssetAsync(Asset asset);
        Task<int> DeleteAssetAsync(Asset asset);
        Task<List<Asset>> GetAssetExportAsync(string search, string hostname, string rowStart, int colStart, string rowEnd, int colEnd);
        Task<bool> AssetIsUniqueAsync(string hostname, Guid id = default);
        Task<List<AssetNetworkPort>> GetNetworkPortExportAsync(string search, string hostname, string rowStart, int colStart, string rowEnd, int colEnd);
        Task<Asset> GetAssetForDecommissioning(Guid assetId);
        Task<AssetNetworkPort> GetNetworkPortAsync(Guid networkPortId);
        Task<PduPort> GetPowerPortAsync(Guid powerPortId);
        Task<int> AddDecomissionedAssetAsync(DecommissionedAsset asset);
        Task<DecommissionedAsset> GetDecommissionedAssetAsync(Guid assetId);
        Task<PagedList<DecommissionedAsset>> GetDecommissionedAssetsAsync(string datacenterName, string generalSearch, string decommissioner,
                    string dateStart, string dateEnd, string rackStart, string rackEnd, string sortBy, string isDesc, int page, int pageSize);
        Task<Asset> GetAsset(int assetNumber);
    }
}
