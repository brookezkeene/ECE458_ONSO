using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IAssetService
    {
        Task<PagedList<AssetDto>> GetAssetsAsync(string search, int page = 1, int pageSize = 10);
        Task<AssetDto> GetAssetAsync(Guid assetId);
        Task<List<AssetDto>> GetAssetExportAsync(AssetExportQuery query);
        Task<List<AssetDto>> GetNetworkPortExportAsync(NetworkPortExportQuery query);
        Task<Guid> CreateAssetAsync(AssetDto assetDto);
        Task DeleteAssetAsync(Guid assetId);
        Task<int> UpdateAssetAsync(AssetDto assetDto);

    }
}