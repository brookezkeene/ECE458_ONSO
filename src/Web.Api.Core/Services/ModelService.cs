using System;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _modelRepository;

        public ModelService(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public async Task<PagedList<ModelDto>> GetModelsAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await _modelRepository.GetModelsAsync(search, page, pageSize);
            return pagedList.ToModel(); // note: ToModel refers to the concept of a model (as opposed to an entity), not the entity "Model"
        }

        public async Task<ModelDto> GetModelAsync(Guid modelId)
        {
            var model = await _modelRepository.GetModelAsync(modelId);
            // TODO: handle null result (no model found)
            return model.ToModel();
        }
    }
}