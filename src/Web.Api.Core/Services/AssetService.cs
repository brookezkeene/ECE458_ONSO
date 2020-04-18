using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Skoruba.AuditLogging.Services;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Events.Asset;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _repository;
        private readonly IAuditEventLogger _auditEventLogger;
        private readonly IMapper _mapper;

        public AssetService(IAssetRepository repository, IAuditEventLogger auditEventLogger, IMapper mapper)
        {
            _repository = repository;
            _auditEventLogger = auditEventLogger;
            _mapper = mapper;
        }

        public async Task<PagedList<AssetDto>> GetAssetsAsync(SearchAssetQuery query)
        {
            query.ToUpper();
            var pagedList = await _repository.GetAssetsAsync(query.Datacenter, query.Vendor, query.ModelNumber, query.Hostname, 
                    query.RackStart, query.RackEnd, query.SortBy, query.IsDesc, query.Page, query.PageSize);
            pagedList.CurrentPage = query.Page;
            return _mapper.Map<PagedList<AssetDto>>(pagedList);
        }

        public async Task<AssetDto> GetAssetAsync(Guid assetId)
        {
            var asset = await _repository.GetAssetAsync(assetId);
            return _mapper.Map<AssetDto>(asset);
        }

        public async Task<AssetDto> GetAssetByNumber(int assetNumber)
        {
            var asset = await _repository.GetAssetByNumber(assetNumber);
            return _mapper.Map<AssetDto>(asset);
        }

        public async Task<List<AssetDto>> GetAssetExportAsync(AssetExportQuery query)
        {
            query = query.ReformatQuery();
            System.Diagnostics.Debug.WriteLine(query.StartRow);
            var assets = await _repository.GetAssetExportAsync(query.Search, query.Hostname, query.StartRow, query.StartCol, query.EndRow, query.EndCol);
            return _mapper.Map<List<AssetDto>>(assets);
        }

        public async Task<List<AssetNetworkPortDto>> GetNetworkPortExportAsync(NetworkPortExportQuery query)
        {
            query = query.ReformatQuery();
            System.Diagnostics.Debug.WriteLine(query.StartRow);
            var assets = await _repository.GetNetworkPortExportAsync(query.Search, query.Hostname, query.StartRow, query.StartCol, query.EndRow, query.EndCol);
            return _mapper.Map<List<AssetNetworkPortDto>>(assets);
        }

        public async Task<Guid> CreateAssetAsync(AssetDto asset)
        {
            var entity = _mapper.Map<Asset>(asset);
            await _repository.AddAssetAsync(entity);

            await _auditEventLogger.LogEventAsync(new AssetCreatedEvent(asset));

            return entity.Id;
        }

        public async Task DeleteAssetAsync(Guid assetId)
        {
            var asset = await _repository.GetAssetAsync(assetId);
            var entity = _mapper.Map<Asset>(asset);
            await _repository.DeleteAssetAsync(entity);

            //await _auditEventLogger.LogEventAsync(new AssetDeletedEvent(asset));
        }

        public async Task DeleteAssetAsync(AssetDto asset)
        {
            var entity = _mapper.Map<Asset>(asset);
            await _repository.DeleteAssetAsync(entity);

            //await _auditEventLogger.LogEventAsync(new AssetDeletedEvent(asset));

        }

        public async Task<List<AssetDto>> GetBlades(Guid chassisId )
        {
            var updated = await _repository.GetBlades(chassisId);
            return _mapper.Map<List<AssetDto>>(updated);
        }

        public async Task<int> UpdateAssetAsync(AssetDto asset)
        {
            var entity = _mapper.Map<Asset>(asset);
            var updated = await _repository.UpdateAssetAsync(entity);

            //await _auditEventLogger.LogEventAsync(new AssetUpdatedEvent(asset));
            return updated;
        }

        public async Task<AssetDto> GetAssetForDecommissioning(Guid assetId)
        {
            var decommissionedAsset = await _repository.GetAssetForDecommissioning(assetId);
            return _mapper.Map<AssetDto>(decommissionedAsset);

        }
        public async Task<PagedList<DecommissionedAssetDto>> GetDecommissionedAssetsAsync(SearchAssetQuery query)
        {
            query.ToUpper();

            var pagedList = await _repository.GetDecommissionedAssetsAsync(query.DatacenterName, query.GeneralSearch, query.Decommissioner,
                    query.DateStart, query.DateEnd, query.RackStart, query.RackEnd, query.SortBy, query.IsDesc, query.Page, query.PageSize);
            pagedList.CurrentPage = query.Page;

            return _mapper.Map<PagedList<DecommissionedAssetDto>>(pagedList);
        }

        public async Task<Guid> CreateDecommissionedAssetAsync(DecommissionedAssetDto asset)
        {
            var entity = _mapper.Map<DecommissionedAsset>(asset);
            await _repository.AddDecomissionedAssetAsync(entity);

            //await _auditEventLogger.LogEventAsync(new AssetCreatedEvent(asset));

            return entity.Id;
        }
        public async Task<DecommissionedAssetDto> GetDecommissionedAssetAsync(Guid assetId)
        {
            var asset = await _repository.GetDecommissionedAssetAsync(assetId);
            return _mapper.Map<DecommissionedAssetDto>(asset);
        }

        private void AddChangePlanItems(Guid changePlanId)
        {

        }
    }
}