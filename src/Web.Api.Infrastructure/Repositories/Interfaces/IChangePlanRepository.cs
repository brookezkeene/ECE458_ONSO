using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IChangePlanRepository
    {
        Task<ChangePlan> GetChangePlanAsync(Guid changePlanId);
        Task<ChangePlanItem> GetChangePlanItemAsync(Guid changePlanItemId);
        Task<List<ChangePlan>> GetChangePlansAsync(Guid? createdById);
        Task<List<ChangePlanItem>> GetChangePlanItemsAsync(Guid changePlanId);
        Task<int> AddChangePlanAsync(ChangePlan changePlan);
        Task<int> AddChangePlanItemAsync(ChangePlanItem changePlanItem);
        Task<int> UpdateChangePlanItemAsync(ChangePlanItem changePlanItem);
        Task<int> DeleteChangePlanAsync(ChangePlan changePlan);
        Task<int> DeleteChangePlanItemAsync(ChangePlanItem changePlanItem);

    }
}
