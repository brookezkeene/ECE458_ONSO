﻿using System;
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
        private readonly IModelRepository _modelRepository;
        private readonly IRackRepository _rackRepository;
        private readonly IIdentityRepository _userRepository;


        public InstanceService(IInstanceRepository repository, IModelRepository modelRepository, IRackRepository rackRepository, IIdentityRepository userRepository)
        {
            _repository = repository;
            _modelRepository = modelRepository;
            _rackRepository = rackRepository;
            _userRepository = userRepository;
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
            query = query.ReformatQuery();
            System.Diagnostics.Debug.WriteLine(query.StartRow);
            var instances = await _repository.GetInstanceExportAsync(query.Search, query.Hostname, query.StartRow, query.StartCol, query.EndRow, query.EndCol) ;
            return instances.ToExportDto();

        }
        public async Task<Guid> CreateInstanceAsync(InstanceDto instanceDto)
        {
            //changing instance entity's newly created duplicate Model/User/Rack entities to
            //point to entities that already exist in the database
            var entity = instanceDto.ToEntity();

            entity.Model = null;
            entity.Model = await _modelRepository.GetModelAsync(instanceDto.Model.Id);
            if (instanceDto.Owner != null)
            {
                entity.Owner = null;
                entity.Owner = await _userRepository.GetUserAsync(instanceDto.Owner.Id);
            }
            //entity.Rack = null;
            //var rack = await _rackRepository.GetRackAsync(entity.Rack.Id);
            //entity.Rack = rack;

            //adding the entity to repository
            await _repository.AddInstanceAsync(entity);
            return entity.Id;
        }
        public async Task DeleteInstanceAsync(Guid instanceId)
        {
            var entity = await _repository.GetInstanceAsync(instanceId);
            await _repository.DeleteInstanceAsync(entity);
        }
        public async Task<int> UpdateInstanceAsync(InstanceDto instanceDto)
        {
            var entity = instanceDto.ToEntity();
            return await _repository.UpdateInstanceAsync(entity);
        }
    }
}