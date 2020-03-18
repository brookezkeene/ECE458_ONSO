using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skoruba.AuditLogging.Services;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Events.Model;
using Web.Api.Core.ExceptionHandling;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    public class ModelService : IModelService
    {
        private readonly IModelRepository _repository;
        private readonly IAuditEventLogger _auditEventLogger;

        public ModelService(IModelRepository repository, IAuditEventLogger auditEventLogger)
        {
            _repository = repository;
            _auditEventLogger = auditEventLogger;
        }

        public async Task<PagedList<ModelDto>> GetModelsAsync(SearchModelQuery query, int page = 1, int pageSize = 10)
        {
            query.ToUpper();
            var pagedList = await _repository.GetModelsAsync(query.Vendor, query.Number, query.HeightStart, query.HeightEnd, 
                query.NetworkRangeStart, query.NetworkRangeEnd, query.PowerRangeStart, query.PowerRangeEnd, 
                query.MemoryRangeStart, query.MemoryRangeEnd, page, pageSize);
            pagedList.CurrentPage = page;
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

        public async Task<int> UpdateModelAsync(ModelDto model)
        {
            var entity = model.ToEntity();
            var updated = await _repository.UpdateModelAsync(entity);

            await _auditEventLogger.LogEventAsync(new ModelUpdatedEvent(model));
            return updated;
        }

        public async Task<int> CreateModelAsync(ModelDto model)
        {
            var entity = model.ToEntity();
            var added = await _repository.AddModelAsync(entity);

            await _auditEventLogger.LogEventAsync(new ModelCreatedEvent(model));
            return added;
        }

        public async Task DeleteModelAsync(ModelDto model)
        {
            var entity = model.ToEntity();
            await _repository.DeleteModelAsync(entity);

            await _auditEventLogger.LogEventAsync(new ModelDeletedEvent(model));
        }
    }
}