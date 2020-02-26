using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Skoruba.AuditLogging.Services;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Events.Datacenter;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;


namespace Web.Api.Core.Services
{
    public class DatacenterService : IDatacenterService
    {
        private readonly IDatacenterRepository _repository;
        private readonly IAuditEventLogger _auditEventLogger;

        public DatacenterService(IDatacenterRepository repository, IAuditEventLogger auditEventLogger)
        {
            _repository = repository;
            _auditEventLogger = auditEventLogger;
        }

        public async Task<Guid> CreateDatacenterAsync(DatacenterDto datacenter)
        {
            var entity = datacenter.ToEntity();
            await _repository.AddDatacenterAsync(entity);

            await _auditEventLogger.LogEventAsync(new DatacenterCreatedEvent(datacenter));

            return entity.Id;
        }

        public async Task DeleteDatacenterAsync(DatacenterDto datacenter)
        {
            var entity = datacenter.ToEntity();
            await _repository.DeleteDatacenterAsync(entity);

            await _auditEventLogger.LogEventAsync(new DatacenterDeletedEvent(datacenter));
        }

        public async Task<DatacenterDto> GetDatacenterAsync(Guid datacenterId)
        {
            var datacenter = await _repository.GetDatacenterAsync(datacenterId);
            // TODO: handle null result (no datacenter found)
            return datacenter.ToDto();
        }
        public async Task<List<AssetNetworkPortDto>> GetNetworkPortsOfDataCenterAsync(Guid datacenterId)
        {
            var networkports = await _repository.GetNetworkPortFromDatacenterAsync(datacenterId);
            return networkports.ToDto();
        }
        public async Task<PagedList<DatacenterDto>> GetDatacentersAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await _repository.GetDatacentersAsync(search, page, pageSize);
            return pagedList.ToDto();
        }

        public async Task<int> UpdateDatacenterAsync(DatacenterDto datacenter)
        {
            var entity = datacenter.ToEntity();
            var updated = await _repository.UpdateDatacenterAsync(entity);

            await _auditEventLogger.LogEventAsync(new DatacenterUpdatedEvent(datacenter));
            return updated;
        }
    }
}
