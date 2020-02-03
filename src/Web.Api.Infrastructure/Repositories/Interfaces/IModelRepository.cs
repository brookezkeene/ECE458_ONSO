using System;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IModelRepository
    {
        Task<PagedList<Model>> GetModelsAsync(string search, int page = 1, int pageSize = 10);
        Task<Model> GetModelAsync(Guid modelId);
        Task<int> AddModelAsync(Model model);
        Task<int> UpdateModelAsync(Model model);
        Task<int> DeleteModelAsync(Model model);
        Task<bool> CanUpdateModelAsync(Model model);
    }
}