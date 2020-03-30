using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IChangePlanRepository
    {
        Task<ChangePlan> GetChangePlanAsync(Guid changePlanId);
        Task<ChangePlanItem> GetChangePlanItemAsync(Guid changePlanItemId);
        Task<ChangePlanItem> GetChangePlanItemAsync(Guid changePlanId, Guid assetId);
        Task<PagedList<ChangePlan>> GetChangePlansAsync(Guid? createdById, int page = 1, int pageSize = 10);
        Task<List<ChangePlanItem>> GetChangePlanItemsAsync(Guid changePlanId);
        Task<List<ChangePlanItem>> GetDecommissionedChangePlanItemsAsync(Guid changePlanId);
        Task<List<ChangePlanItem>> GetAssetChangePlanItemsAsync(Guid changePlanId);
        Task<int> AddChangePlanAsync(ChangePlan changePlan);
        Task<int> AddChangePlanItemAsync(ChangePlanItem changePlanItem);
        Task<int> UpdateChangePlanAsync(ChangePlan changePlan);
        Task<int> UpdateChangePlanItemAsync(ChangePlanItem changePlanItem);
        Task<int> DeleteChangePlanAsync(ChangePlan changePlan);
        Task<int> DeleteChangePlanItemAsync(ChangePlanItem changePlanItem);
        Task<int> ExecuteChangePlan(List<ChangePlanItem> changePlanItems);

    }
}
