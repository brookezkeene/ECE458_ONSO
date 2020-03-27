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
        public ChangePlanService(IChangePlanRepository repository)
        {
            _repository = repository;
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
        public async Task ExecuteChangePlan(List<ChangePlanItemDto> changePlanItems)
        {
            List<ChangePlanItem> changePlanItemsEntities = new List<ChangePlanItem>();
            for (int i = 0; i < changePlanItems.Count; i++)
            {
                var changePlanItem = changePlanItems[i];
                //NOTE: THE NEWDATA HERE IS A CreateAssetApiDto
                if (changePlanItem.ExecutionType.Equals("create") || changePlanItem.ExecutionType.Equals("update"))
                {
                    var assetDto = JsonConvert.DeserializeObject<AssetDto>(changePlanItem.NewData);
                    var asset = assetDto.ToEntity();
                    changePlanItem.NewData = JsonConvert.SerializeObject(asset);
                    var changePlanItemEntity = changePlanItem.ToEntity();
                    changePlanItemsEntities.Add(changePlanItemEntity);
                }
                //NOTE: THE NEWDATA HERE IS A DecommissionedAssetQuery
                else if (changePlanItem.ExecutionType.Equals("decommission"))
                {
                    var decommissionedAssetDto = JsonConvert.DeserializeObject<DecommissionedAssetDto>(changePlanItem.NewData);
                    var decommissionedAsset = decommissionedAssetDto.ToEntity();
                    changePlanItem.NewData = JsonConvert.SerializeObject(decommissionedAsset);
                    var changePlanItemEntity = changePlanItem.ToEntity();
                    changePlanItemsEntities.Add(changePlanItemEntity);
                }
            }
            await _repository.ExecuteChangePlan(changePlanItemsEntities);
        }
    }
}
