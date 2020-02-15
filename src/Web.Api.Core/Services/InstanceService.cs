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
        private readonly IRackRepository _rackRepository;

        public InstanceService(IInstanceRepository repository, IRackRepository rackRepository)
        {
            _repository = repository;
            _rackRepository = rackRepository;
        }

        public async Task<PagedList<AssetDto>> GetInstancesAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await _repository.GetInstancesAsync(search, page, pageSize);
            return pagedList.ToDto();
        }

        public async Task<AssetDto> GetInstanceAsync(Guid instanceId)
        {
            var instance = await _repository.GetInstanceAsync(instanceId);
            return instance.ToDto();
        }
        public async Task<List<AssetDto>> GetInstanceExportAsync(InstanceExportQuery query)
        {
            query = query.ReformatQuery();
            System.Diagnostics.Debug.WriteLine(query.StartRow);
            var instances = await _repository.GetInstanceExportAsync(query.Search, query.Hostname, query.StartRow, query.StartCol, query.EndRow, query.EndCol) ;
            return instances.ToDto();

        }
        public async Task<Guid> CreateInstanceAsync(AssetDto instanceDto)
        {
            //changing instance entity's newly created duplicate Model/User/Rack entities to
            //point to entities that already exist in the database
            var entity = instanceDto.ToEntity();
            entity.Rack = await _rackRepository.GetRackAsync(entity.Rack.Row, entity.Rack.Column); // TODO: review rack ID references

            await _repository.AddInstanceAsync(entity);
            return entity.Id;
        }
        public async Task DeleteInstanceAsync(Guid instanceId)
        {
            var entity = await _repository.GetInstanceAsync(instanceId);
            await _repository.DeleteInstanceAsync(entity);
        }
        public async Task<int> UpdateInstanceAsync(AssetDto instanceDto)
        {
            var entity = instanceDto.ToEntity();
            entity.Rack = await _rackRepository.GetRackAsync(entity.Rack.Row, entity.Rack.Column); // TODO: review rack ID references

            return await _repository.UpdateInstanceAsync(entity);
        }
    }
}