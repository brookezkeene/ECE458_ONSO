using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IChangePlanService
    {
        Task<Guid> CreateChangePlanAsync(ChangePlanDto changePlan);
        Task<Guid> CreateChangePlanItemAsync(ChangePlanItemDto changePlanItem);
    }
}
