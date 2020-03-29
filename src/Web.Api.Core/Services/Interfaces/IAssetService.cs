using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IAssetService
    {
        Task<PagedList<AssetDto>> GetAssetsAsync(SearchAssetQuery query);
        Task<AssetDto> GetAssetAsync(Guid assetId);
        Task<List<AssetDto>> GetAssetExportAsync(AssetExportQuery query);
        Task<List<AssetNetworkPortDto>> GetNetworkPortExportAsync(NetworkPortExportQuery query);
        Task<Guid> CreateAssetAsync(AssetDto asset);
        Task DeleteAssetAsync(Guid assetId);
        Task DeleteAssetAsync(AssetDto asset);
        Task<int> UpdateAssetAsync(AssetDto asset);
        Task<AssetDto> GetAssetForDecommissioning(Guid assetId);
        Task<Guid> CreateDecommissionedAssetAsync(DecommissionedAssetDto asset);
        Task<DecommissionedAssetDto> GetDecommissionedAssetAsync(Guid assetId);
        Task<PagedList<DecommissionedAssetDto>> GetDecommissionedAssetsAsync(SearchAssetQuery query);
    }
}