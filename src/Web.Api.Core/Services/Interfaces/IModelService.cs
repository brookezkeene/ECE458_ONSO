using System;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IModelService
    {
        Task<PagedList<FlatModelDto>> GetModelsAsync(string search, int page = 1, int pageSize = 10);
        Task<ModelDto> GetModelAsync(Guid modelId);
        Task UpdateModelAsync(ModelDto modelDto);
        Task<Guid> CreateModelAsync(ModelDto modelDto);
        Task DeleteModelAsync(Guid modelId);
    }
}
