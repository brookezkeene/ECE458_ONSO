using System;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IInstanceService
    {
        Task<PagedList<InstanceDto>> GetInstancesAsync(string search, int page = 1, int pageSize = 10);
        Task<InstanceDto> GetInstanceAsync(Guid instanceId);
    }

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
    }
}