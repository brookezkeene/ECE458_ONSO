using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.ExceptionHandling;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _repository;

        public ModelService(IModelRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedList<ModelDto>> GetModelsAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await _repository.GetModelsAsync(search, page, pageSize);
            return pagedList.ToDto();
        }

        public async Task<List<ModelDto>> GetModelExportAsync(ModelExportQuery query)
        {
            query = query.ToUpper();
            var models = await _repository.GetModelExportAsync(query.Search);
            return models.ToDto();
        }

        public async Task<ModelDto> GetModelAsync(Guid modelId)
        {
            var model = await _repository.GetModelAsync(modelId);
            // TODO: handle null result (no model found)
            return model.ToDto();
        }

        public async Task<int> UpdateModelAsync(ModelDto modelDto)
        {
            var model = modelDto.ToEntity();
            var updated = await _repository.UpdateModelAsync(model);

            return updated;
        }

        public async Task<Guid> CreateModelAsync(ModelDto modelDto)
        {
            var entity = modelDto.ToEntity();
            await _repository.AddModelAsync(entity);
            return entity.Id;
        }

        public async Task DeleteModelAsync(Guid modelId)
        {
            var entity = await _repository.GetModelAsync(modelId);
            await _repository.DeleteModelAsync(entity);
        }
    }
}