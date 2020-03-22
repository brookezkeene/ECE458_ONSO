using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;


namespace Web.Api.Core.Services.Interfaces
{
    public interface IModelService
    {
        Task<PagedList<ModelDto>> GetModelsAsync(SearchModelQuery query, int page = 1, int pageSize = 10);
        Task<List<ModelDto>> GetModelExportAsync(ModelExportQuery query);
        Task<ModelDto> GetModelAsync(Guid modelId);
        Task<int> UpdateModelAsync(ModelDto model);
        Task<int> CreateModelAsync(ModelDto model);
        Task DeleteModelAsync(ModelDto model);
    }
}
