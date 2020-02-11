using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    public class InstanceService : IInstanceService
    {
        private readonly IInstanceRepository _repository;

        public InstanceService(IInstanceRepository repository)
        {
            _repository = repository;
        }

        public async Task<PagedList<InstanceDto>> GetInstancesAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await _repository.GetInstancesAsync(search, page, pageSize);
            return pagedList.ToDto();
        }

        public async Task<InstanceDto> GetInstanceAsync(Guid instanceId)
        {
            var instance = await _repository.GetInstanceAsync(instanceId);
            return instance.ToDto();
        }
        public async Task<List<ExportInstanceDto>> GetInstanceExportAsync(InstanceExportQuery query)
        {
            query = query.reformatQuery();
            System.Diagnostics.Debug.WriteLine(query.StartRow);
            var instances = await _repository.GetInstanceExportAsync(query.Search, query.StartRow, query.StartCol, query.EndRow, query.EndCol) ;
            return instances.ToExportDto();

        }
    }
}