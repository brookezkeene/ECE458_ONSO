using System;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;
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

        public async Task<ModelDto> GetModelAsync(Guid modelId)
        {
            var model = await _repository.GetModelAsync(modelId);
            // TODO: handle null result (no model found)
            return model.ToDto();
        }

        public async Task UpdateModelAsync(ModelDto modelDto)
        {
            var canInsert = await CanUpdateModelAsync(modelDto);
            if (!canInsert)
            {
                throw new InvalidOperationException();
            }

            var entity = modelDto.ToEntity();
            await _repository.UpdateModelAsync(entity);
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

        private async Task<bool> CanUpdateModelAsync(ModelDto modelDto)
        {
            var entity = modelDto.ToEntity();
            return await _repository.CanUpdateModelAsync(entity);
        }
    }
}