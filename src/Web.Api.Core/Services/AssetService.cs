using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _repository;

        public AssetService(IAssetRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedList<AssetDto>> GetAssetsAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await _repository.GetAssetsAsync(search, page, pageSize);
            return pagedList.ToDto();
        }

        public async Task<AssetDto> GetAssetAsync(Guid assetId)
        {
            var asset = await _repository.GetAssetAsync(assetId);
            return asset.ToDto();
        }
        public async Task<List<AssetDto>> GetAssetExportAsync(AssetExportQuery query)
        {
            query = query.ReformatQuery();
            System.Diagnostics.Debug.WriteLine(query.StartRow);
            var assets = await _repository.GetAssetExportAsync(query.Search, query.Hostname, query.StartRow, query.StartCol, query.EndRow, query.EndCol) ;
            return assets.ToDto();

        }
        public async Task<List<AssetNetworkPortDto>> GetNetworkPortExportAsync(NetworkPortExportQuery query)
        {
            query = query.ReformatQuery();
            System.Diagnostics.Debug.WriteLine(query.StartRow);
            var assets = await _repository.GetNetworkPortExportAsync(query.Search, query.Hostname, query.StartRow, query.StartCol, query.EndRow, query.EndCol);
            return assets.ToDto();
        }
        public async Task<Guid> CreateAssetAsync(AssetDto assetDto)
        {
            var entity = assetDto.ToEntity();

            await _repository.AddAssetAsync(entity);
            return entity.Id;
        }
        public async Task DeleteAssetAsync(Guid assetId)
        {
            var entity = await _repository.GetAssetAsync(assetId);
            await _repository.DeleteAssetAsync(entity);
        }
        public async Task<int> UpdateAssetAsync(AssetDto assetDto)
        {
            var entity = assetDto.ToEntity();
            return await _repository.UpdateAssetAsync(entity);
        }
    }
}