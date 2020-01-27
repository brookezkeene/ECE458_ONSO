using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Common;

namespace Web.Api.Infrastructure
{
    public interface IModelRepository
    {
        Task<PagedList<Model>> GetModelsAsync(string search, int page = 1, int pageSize = 10);
        Task<Model> GetModelAsync(Guid modelId);
        Task<int> AddModelAsync(Model model);
        Task<int> UpdateModelAsync(Model model);
        Task<int> DeleteModelAsync(Model model);
    }

    public interface IRackRepository
    {
        Task<PagedList<Rack>> GetRacksAsync(string search, int page = 1, int pageSize = 10);
        Task<List<Rack>> GetRacksInRangeAsync(string rowStart, int colStart, string rowEnd, int colEnd);
        Task<Rack> GetRackAsync(Guid rackId);
        Task<int> AddRackAsync(Rack rack);
        Task<int> UpdateRackAsync(Rack rack);
        Task<int> DeleteRackAsync(Rack rack);
    }

    public interface IInstanceRepository
    {
        Task<PagedList<Instance>> GetInstancesAsync(string search, int page = 1, int pageSize = 10);
        Task<Instance> GetInstanceAsync(Guid instanceId);
        Task<int> AddInstanceAsync(Instance instance);
        Task<int> UpdateInstanceAsync(Instance instance);
        Task<int> DeleteInstanceAsync(Instance instance);
    }

}
