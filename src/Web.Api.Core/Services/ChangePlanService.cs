using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public ChangePlanService(IChangePlanRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ChangePlanDto> GetChangePlanAsync(Guid changPlanId)
        {
            var changePlan = await _repository.GetChangePlanAsync(changPlanId);
            return _mapper.Map<ChangePlanDto>(changePlan);
        }

        public async Task<ChangePlanItemDto> GetChangePlanItemAsync(Guid changePlanItemId)
        {
            var changePlanItem = await _repository.GetChangePlanItemAsync(changePlanItemId);
            return _mapper.Map<ChangePlanItemDto>(changePlanItem);
        }

        public async Task<PagedList<ChangePlanDto>> GetChangePlansAsync(Guid? createdById, int page = 1, int pageSize = 10)
        {
            var list = await _repository.GetChangePlansAsync(createdById, page, pageSize);
            return _mapper.Map<PagedList<ChangePlanDto>>(list);
        }

        public async Task<List<ChangePlanItemDto>> GetChangePlanItemsAsync(Guid changePlanId)
        {
            var list = await _repository.GetChangePlanItemsAsync(changePlanId);
            return _mapper.Map<List<ChangePlanItemDto>>(list);
        }
        public async Task<List<ChangePlanItemDto>> GetDecommissionedChangePlanItemsAsync(Guid changePlanId)
        {
            var list = await _repository.GetDecommissionedChangePlanItemsAsync(changePlanId);
            return _mapper.Map<List<ChangePlanItemDto>>(list);
        }
        public async Task<List<ChangePlanItemDto>> GetAsssetChangePlanItemsAsync(Guid changePlanId)
        {
            var list = await _repository.GetAssetChangePlanItemsAsync(changePlanId);
            return _mapper.Map<List<ChangePlanItemDto>>(list);
        }
        public async Task<Guid> CreateChangePlanAsync(ChangePlanDto changePlan)
        {
            var entity = _mapper.Map<ChangePlan>(changePlan);
            await _repository.AddChangePlanAsync(entity);
            return entity.Id;
        }

        public async Task<Guid> CreateChangePlanItemAsync(ChangePlanItemDto changePlanItem)
        {
            var entity = _mapper.Map<ChangePlanItem>(changePlanItem);
            await _repository.AddChangePlanItemAsync(entity);
            return entity.Id;
        }

        public async Task<int> UpdateChangePlanItemAsync(ChangePlanItemDto changePlanItem)
        {
            var entity = _mapper.Map<ChangePlanItem>(changePlanItem);
            var updated = await _repository.UpdateChangePlanItemAsync(entity);
            return updated;
        }

        public async Task<int> UpdateChangePlanAsync(ChangePlanDto changePlan)
        {
            var entity = _mapper.Map<ChangePlan>(changePlan);
            var updated = await _repository.UpdateChangePlanAsync(entity);
            return updated;
        }

        public async Task DeleteChangePlanAsync(ChangePlanDto changePlan)
        {
            var entity = _mapper.Map<ChangePlan>(changePlan);
            await _repository.DeleteChangePlanAsync(entity);
        }

        public async Task DeleteChangePlanItemAsync(ChangePlanItemDto changePlanItem)
        {
            var entity = _mapper.Map<ChangePlanItem>(changePlanItem);
            await _repository.DeleteChangePlanItemAsync(entity);
        }

        public async Task<int> ExecuteChangePlan(List<ChangePlanItemDto> changePlanItems)
        {
            var changePlanItemsEntities = _mapper.Map<List<ChangePlanItem>>(changePlanItems);
            return await _repository.ExecuteChangePlan(changePlanItemsEntities);
        }
       
    }
}
