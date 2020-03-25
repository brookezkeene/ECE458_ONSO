using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
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
    }
}
