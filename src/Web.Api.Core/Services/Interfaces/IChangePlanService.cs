using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IChangePlanService
    {
        Task<ChangePlanDto> GetChangePlanAsync(Guid changPlanId);
        Task<ChangePlanItemDto> GetChangePlanItemAsync(Guid changePlanItemId);
        Task<List<ChangePlanDto>> GetChangePlansAsync(Guid? createdById);
        Task<List<ChangePlanItemDto>> GetChangePlanItemsAsync(Guid changePlanId);
        Task<Guid> CreateChangePlanAsync(ChangePlanDto changePlan);
        Task<Guid> CreateChangePlanItemAsync(ChangePlanItemDto changePlanItem);
        Task<int> UpdateChangePlanItemAsync(ChangePlanItemDto changePlanItem);
        Task DeleteChangePlanAsync(ChangePlanDto changePlan);
        Task DeleteChangePlanItemAsync(ChangePlanItemDto changePlanItem);
    }
}
