using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Skoruba.AuditLogging.Services;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Events.Datacenter;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;


namespace Web.Api.Core.Services
{
    public class DatacenterService : IDatacenterService
    {
        private readonly IDatacenterRepository _repository;
        private readonly IAuditEventLogger _auditEventLogger;
        private readonly IMapper _mapper;

        public DatacenterService(IDatacenterRepository repository, IAuditEventLogger auditEventLogger, IMapper mapper)
        {
            _repository = repository;
            _auditEventLogger = auditEventLogger;
            _mapper = mapper;
        }

        public async Task<Guid> CreateDatacenterAsync(DatacenterDto datacenter)
        {
            var entity = _mapper.Map<Datacenter>(datacenter);
            await _repository.AddDatacenterAsync(entity);

            await _auditEventLogger.LogEventAsync(new DatacenterCreatedEvent(datacenter));

            return entity.Id;
        }

        public async Task DeleteDatacenterAsync(Guid datacenterId)
        {
            var entity = await _repository.GetDatacenterAsync(datacenterId);
            await _repository.DeleteDatacenterAsync(entity);

            //await _auditEventLogger.LogEventAsync(new DatacenterDeletedEvent(datacenterId));
        }

        public async Task<List<AssetNetworkPortDto>> GetOpenNetworkPortsOfDatacenterAsync(Guid datacenterId)
        {
            var ports = await _repository.GetOpenNetworkPortsFromDatacenterAsync(datacenterId);
            return _mapper.Map<List<AssetNetworkPortDto>>(ports);
        }

        public async Task<DatacenterDto> GetDatacenterAsync(Guid datacenterId)
        {
            var datacenter = await _repository.GetDatacenterAsync(datacenterId);
            // TODO: handle null result (no datacenter found)
            return _mapper.Map<DatacenterDto>(datacenter);
        }

        public async Task<List<AssetNetworkPortDto>> GetNetworkPortsOfDataCenterAsync(Guid datacenterId)
        {
            var networkports = await _repository.GetNetworkPortFromDatacenterAsync(datacenterId);
            return _mapper.Map<List<AssetNetworkPortDto>>(networkports);
        }
        public async Task<List<AssetDto>> GetChassisOfDataCenterAsync(Guid datacenterId)
        {
            var networkports = await _repository.GetChassisFromDatacenterAsync(datacenterId);
            return _mapper.Map<List<AssetDto>>(networkports);
        }
        public async Task<PagedList<DatacenterDto>> GetDatacentersAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await _repository.GetDatacentersAsync(search, page, pageSize);
            return _mapper.Map<PagedList<DatacenterDto>>(pagedList);
        }

        public async Task<int> UpdateDatacenterAsync(DatacenterDto datacenter)
        {
            var entity = _mapper.Map<Datacenter>(datacenter);
            var updated = await _repository.UpdateDatacenterAsync(entity);

            //await _auditEventLogger.LogEventAsync(new DatacenterUpdatedEvent(datacenter));
            return updated;
        }
    }
}
