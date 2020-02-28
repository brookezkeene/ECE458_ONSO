using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Skoruba.AuditLogging.Services;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Events.Rack;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Services
{
    public class RackService : IRackService
    {
        private readonly IRackRepository _rackRepository;
        private readonly IAuditEventLogger _auditEventLogger;

        public RackService(IRackRepository rackRepository, IAuditEventLogger auditEventLogger)
        {
            _rackRepository = rackRepository;
            _auditEventLogger = auditEventLogger;
        }

        public async Task<List<RackDto>> GetRacksAsync(RackRangeQuery query)
        {
            query = query.ToUpper();
            var racks = await _rackRepository.GetRacksInRangeAsync(query.StartRow, query.StartCol, query.EndRow, query.EndCol, query.DatacenterId);
            return racks.ToDto();
        }

        public async Task<PagedList<RackDto>> GetRacksAsync(Guid? datacenterId, int page = 1, int pageSize = 10)
        {
            var racks = await _rackRepository.GetRacksAsync(datacenterId, page, pageSize);
            return racks.ToDto();
        }

        public async Task CreateRacksAsync(RackRangeQuery query)
        {
            query = query.ToUpper();
            await _rackRepository.CreateRacksInRangeAsync(query.StartRow, query.StartCol, query.EndRow, query.EndCol, query.DatacenterId);

            await _auditEventLogger.LogEventAsync(new RackCreatedEvent(query));
        }

        public async Task DeleteRacksAsync(RackRangeQuery query)
        {
            query = query.ToUpper(); 
            await _rackRepository.DeleteRacksInRangeAsync(query.StartRow, query.StartCol, query.EndRow, query.EndCol, query.DatacenterId);

            await _auditEventLogger.LogEventAsync(new RackDeletedEvent(query));
        }

        public async Task<RackDto> GetAvailablePowerPorts(Guid id)
        {
            var rack = await _rackRepository.GetRackAsync(id);

            // include only ports without a connection
            rack.Pdus.ForEach(pdu => pdu.Ports = pdu.Ports.Where(port => port.AssetPowerPortId is null)
                .ToList());

            return rack.ToDto();
        }
    }
}