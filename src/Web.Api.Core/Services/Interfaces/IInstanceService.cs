using System;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IInstanceService
    {
        Task<PagedList<InstanceDto>> GetInstancesAsync(string search, int page = 1, int pageSize = 10);
        Task<InstanceDto> GetInstanceAsync(Guid instanceId);
    }
}