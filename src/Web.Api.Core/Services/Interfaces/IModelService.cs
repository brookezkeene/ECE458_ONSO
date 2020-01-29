using System;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IModelService
    {
        Task<PagedList<ModelDto>> GetModelsAsync(string search, int page = 1, int pageSize = 10);
        Task<ModelDto> GetModelAsync(Guid modelId);
    }
}
