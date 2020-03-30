using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IChangePlanService
    {
        Task<ChangePlanDto> GetChangePlanAsync(Guid changPlanId);
        Task<ChangePlanItemDto> GetChangePlanItemAsync(Guid changePlanItemId);
        Task<PagedList<ChangePlanDto>> GetChangePlansAsync(Guid? createdById, int page = 1, int pageSize = 10);
        Task<List<ChangePlanItemDto>> GetChangePlanItemsAsync(Guid changePlanId);
        Task<ChangePlanItemDto> GetChangePlanItemAsync(Guid changePlanId, Guid assetId);
        Task<List<ChangePlanItemDto>> GetDecommissionedChangePlanItemsAsync(Guid changePlanId);
        Task<List<ChangePlanItemDto>> GetAsssetChangePlanItemsAsync(Guid changePlanId);
        Task<Guid> CreateChangePlanAsync(ChangePlanDto changePlan);
        Task<Guid> CreateChangePlanItemAsync(ChangePlanItemDto changePlanItem);
        Task<int> UpdateChangePlanAsync(ChangePlanDto changePlan);
        Task<int> UpdateChangePlanItemAsync(ChangePlanItemDto changePlanItem);
        Task DeleteChangePlanAsync(ChangePlanDto changePlan);
        Task DeleteChangePlanItemAsync(ChangePlanItemDto changePlanItem);
        Task<int> ExecuteChangePlan(List<ChangePlanItemDto> changePlanItems);
        Task<AssetDto> FillFieldsInAssetApiForChangePlans(AssetDto assetDto);
    }
}
