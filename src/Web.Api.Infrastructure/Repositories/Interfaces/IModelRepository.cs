using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IModelRepository
    {
        Task<PagedList<Model>> GetModelsAsync(string search, int page = 1, int pageSize = 10);
        Task<List<Model>> GetModelExportAsync(string search);
        Task<Model> GetModelAsync(Guid modelId);
        Task<int> AddModelAsync(Model model);
        Task<int> UpdateModelAsync(Model model);
        Task<int> DeleteModelAsync(Model model);
        Task<bool> CanUpdateModelAsync(Model model);
        Task<bool> AssetsOfModelExistAsync(Model model);
        Task<bool> ModelIsUniqueAsync(string vendor, string modelNumber, Guid id = default);
        Task<bool> MeetsHeightChangeCriteriaAsync(Model model);
        Task<bool> ModelExistsAsync(string vendor, string modelNumber, Guid id);
        Model GetModel(string vendor, string modelNumber);
    }
}