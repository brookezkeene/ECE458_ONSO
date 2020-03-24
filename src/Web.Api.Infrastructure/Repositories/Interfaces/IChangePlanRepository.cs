using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IChangePlanRepository
    {
        Task<int> AddChangePlanAsync(ChangePlan changePlan);
        Task<int> AddChangePlanItemAsync(ChangePlanItem changePlanItem);
    }
}
