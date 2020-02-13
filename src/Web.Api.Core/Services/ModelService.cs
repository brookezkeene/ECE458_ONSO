using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.ExceptionHandling;
using Web.Api.Core.Mappers;
using Web.Api.Core.Resources;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _repository;
        private readonly IModelServiceResources _resources;

        public ModelService(IModelRepository repository, IModelServiceResources resources)
        {
            _repository = repository;
            _resources = resources;
        }

        public async Task<PagedList<FlatModelDto>> GetModelsAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await _repository.GetModelsAsync(search, page, pageSize);
            return pagedList.ToFlatDto();
        }

        public async Task<List<ExportModelDto>> GetModelExportAsync(ModelExportQuery query)
        {
            query = query.ToUpper();
            var models = await _repository.GetModelExportAsync(query.Search);
            return models.ToExportDto();
        }

        public async Task<ModelDto> GetModelAsync(Guid modelId)
        {
            var model = await _repository.GetModelAsync(modelId);
            // TODO: handle null result (no model found)
            return model.ToDto();
        }

        public async Task<int> UpdateModelAsync(FlatModelDto modelDto)
        {
            if (!await CanUpdateModelAsync(modelDto))
            {
                var error = _resources.GeneralConstraintViolation();
                throw new UserFriendlyException(error.Description, error.Code, modelDto);
            }

            var model = modelDto.ToEntity();
            var updated = await _repository.UpdateModelAsync(model);

            return updated;
        }

        public async Task<Guid> CreateModelAsync(FlatModelDto modelDto)
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

        private async Task<bool> CanUpdateModelAsync(FlatModelDto modelDto)
        {
            var entity = modelDto.ToEntity();
            return await _repository.CanUpdateModelAsync(entity);
        }
    }
}