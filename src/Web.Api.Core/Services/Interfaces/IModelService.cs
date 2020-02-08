using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IModelService
    {
        Task<PagedList<FlatModelDto>> GetModelsAsync(string search, int page = 1, int pageSize = 10);
        Task<List<FlatModelDto>> GetModelExportAsync(ExportQuery query);
        Task<ModelDto> GetModelAsync(Guid modelId);
        Task<int> UpdateModelAsync(FlatModelDto modelDto);
        Task<Guid> CreateModelAsync(FlatModelDto modelDto);
        Task DeleteModelAsync(Guid modelId);
    }
}
