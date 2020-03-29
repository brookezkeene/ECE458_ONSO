using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    
    public class ChangePlanService : IChangePlanService
    {
        private readonly IChangePlanRepository _repository;
        private readonly IAssetRepository _assetRepository;
        private readonly IModelRepository _modelRepository;
        private readonly IRackRepository _rackRepository;
        private readonly IIdentityRepository _ownerRepository;

        public ChangePlanService(IChangePlanRepository repository, IAssetRepository assetRepository, 
            IModelRepository modelRepository, IRackRepository rackRepository, IIdentityRepository ownerRepository)
        {
            _repository = repository;
            _assetRepository = assetRepository;
            _modelRepository = modelRepository;
            _rackRepository = rackRepository;
            _ownerRepository = ownerRepository;
        }
        public async Task<ChangePlanDto> GetChangePlanAsync(Guid changPlanId)
        {
            var changePlan = await _repository.GetChangePlanAsync(changPlanId);
            return changePlan.ToDto();
        }
        public async Task<ChangePlanItemDto> GetChangePlanItemAsync(Guid changePlanItemId)
        {
            var changePlanItem = await _repository.GetChangePlanItemAsync(changePlanItemId);
            return changePlanItem.ToDto();
        }
        public async Task<PagedList<ChangePlanDto>> GetChangePlansAsync(Guid? createdById, int page = 1, int pageSize = 10)
        {
            var list = await _repository.GetChangePlansAsync(createdById, page, pageSize);
            return list.ToDto();
        }
        public async Task<List<ChangePlanItemDto>> GetChangePlanItemsAsync(Guid changePlanId)
        {
            var list = await _repository.GetChangePlanItemsAsync(changePlanId);
            return list.ToDto();
        }
        public async Task<Guid> CreateChangePlanAsync(ChangePlanDto changePlan)
        {
            var entity = changePlan.ToEntity();
            await _repository.AddChangePlanAsync(entity);
            return entity.Id;
        }
        public async Task<Guid> CreateChangePlanItemAsync(ChangePlanItemDto changePlanItem)
        {
            var entity = changePlanItem.ToEntity();
            await _repository.AddChangePlanItemAsync(entity);
            return entity.Id;
        }
        public async Task<Guid> CreateChangePlanItemAsync(AssetDto assetDto, Guid changePlanId)
        {
            var asset = assetDto.ToEntity();
            assetDto.Model = (await _modelRepository.GetModelAsync(assetDto.ModelId)).ToDto();
            assetDto.Rack = (await _rackRepository.GetRackAsync(assetDto.RackId)).ToDto();
            var changePlanItemDto = new ChangePlanItemDto
            {
                ChangePlanId = changePlanId,
                ExecutionType = "create",
                NewData = JsonConvert.SerializeObject(assetDto),
                CreatedDate = DateTime.Now
            };
            var entity = changePlanItemDto.ToEntity();
            await _repository.AddChangePlanItemAsync(entity);
            
            return entity.Id;
        }
        public async Task<Guid> UpdateChangePlanItemAsync(AssetDto assetDto, Guid changePlanId)
        {
            var asset = assetDto.ToEntity();
            var originalAsset = _assetRepository.GetAssetAsync(assetDto.Id);

            var changePlanItemDto = new ChangePlanItemDto
            {
                ChangePlanId = changePlanId,
                AssetId = asset.Id,
                NewData = JsonConvert.SerializeObject(asset),
                PreviousData = JsonConvert.SerializeObject(originalAsset),
                ExecutionType = "update",
                CreatedDate = DateTime.Now
            };
            var entity = changePlanItemDto.ToEntity();
            await _repository.AddChangePlanItemAsync(entity);
            return entity.Id;
        }
        public async Task<Guid> DecommisionChangePlanItemAsync(DecommissionedAssetDto assetDto, Guid changePlanId)
        {
            var asset = assetDto.ToEntity();
            var originalAsset = _assetRepository.GetAssetAsync(assetDto.Id);

            var changePlanItemDto = new ChangePlanItemDto
            {
                ChangePlanId = changePlanId,
                AssetId = asset.Id,
                NewData = JsonConvert.SerializeObject(asset),
                PreviousData = JsonConvert.SerializeObject(originalAsset),
                ExecutionType = "decommission",
                CreatedDate = DateTime.Now
            };
            var entity = changePlanItemDto.ToEntity();
            await _repository.AddChangePlanItemAsync(entity);
            return entity.Id;
        }
        public async Task<int> UpdateChangePlanItemAsync(ChangePlanItemDto changePlanItem)
        {
            var entity = changePlanItem.ToEntity();
            var updated = await _repository.UpdateChangePlanItemAsync(entity);
            return updated;
        }
        public async Task<int> UpdateChangePlanAsync(ChangePlanDto changePlan)
        {
            var entity = changePlan.ToEntity();
            var updated = await _repository.UpdateChangePlanAsync(entity);
            return updated;
        }
        public async Task DeleteChangePlanAsync(ChangePlanDto changePlan)
        {
            var entity = changePlan.ToEntity();
            await _repository.DeleteChangePlanAsync(entity);
        }
        public async Task DeleteChangePlanItemAsync(ChangePlanItemDto changePlanItem)
        {
            var entity = changePlanItem.ToEntity();
            await _repository.DeleteChangePlanItemAsync(entity);
        }
        public async Task<int> ExecuteChangePlan(List<ChangePlanItemDto> changePlanItems)
        {
            List<ChangePlanItem> changePlanItemsEntities = new List<ChangePlanItem>();
            for (int i = 0; i < changePlanItems.Count; i++)
            {
                var changePlanItemEntity = changePlanItems[i].ToEntity();
                changePlanItemsEntities.Add(changePlanItemEntity);
            }
            return await _repository.ExecuteChangePlan(changePlanItemsEntities);
        }
    }
}
