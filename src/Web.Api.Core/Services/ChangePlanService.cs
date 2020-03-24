using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    }
}
