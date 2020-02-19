using System;
using System.Collections.Generic;
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
        private readonly IRackRepository _rackRepository;

        public AssetService(IAssetRepository repository, IRackRepository rackRepository)
        {
            _repository = repository;
            _rackRepository = rackRepository;
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
        public async Task<Guid> CreateAssetAsync(AssetDto assetDto)
        {
            //changing asset entity's newly created duplicate Model/User/Rack entities to
            //point to entities that already exist in the database
            var entity = assetDto.ToEntity();
            //entity.Rack = await _rackRepository.GetRackAsync(entity.Rack.Row, entity.Rack.Column); // TODO: review rack ID references

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
            //entity.Rack = await _rackRepository.GetRackAsync(entity.Rack.Row, entity.Rack.Column); // TODO: review rack ID references

            return await _repository.UpdateAssetAsync(entity);
        }
    }
}