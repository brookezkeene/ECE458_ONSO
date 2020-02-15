using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IInstanceRepository
    {
        Task<PagedList<Instance>> GetInstancesAsync(string search, int page = 1, int pageSize = 10);
        Task<Instance> GetInstanceAsync(Guid instanceId);
        Task<int> AddInstanceAsync(Instance instance);
        Task<int> UpdateInstanceAsync(Instance instance);
        Task<int> DeleteInstanceAsync(Instance instance);
        Task<List<Instance>> GetInstanceExportAsync(string search, string hostname, string rowStart, int colStart, string rowEnd, int colEnd);
        Task<bool> InstanceIsUniqueAsync(string hostname, Guid id = default);
    }
}
