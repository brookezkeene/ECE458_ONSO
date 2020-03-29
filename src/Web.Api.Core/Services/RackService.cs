using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public RackService(IRackRepository rackRepository, IAuditEventLogger auditEventLogger, IMapper mapper)
        {
            _rackRepository = rackRepository;
            _auditEventLogger = auditEventLogger;
            _mapper = mapper;
        }

        public async Task<List<RackDto>> GetRacksAsync(RackRangeQuery query)
        {
            query = query.ToUpper();
            var racks = await _rackRepository.GetRacksInRangeAsync(query.StartRow, query.StartCol, query.EndRow, query.EndCol, query.DatacenterId);
            return _mapper.Map<List<RackDto>>(racks);
        }

        public async Task<PagedList<RackDto>> GetRacksAsync(SearchRackQuery query)
        {
            var pagedList = await _rackRepository.GetRacksAsync(query.Datacenter, query.SortBy, query.IsDesc, query.Page, query.PageSize);
            pagedList.CurrentPage = query.Page;
            return _mapper.Map<PagedList<RackDto>>(pagedList);
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
            rack.Pdus.ForEach(pdu => pdu.Ports = pdu.Ports.Where(port => port.PowerConnection is null)
                .ToList());

            return _mapper.Map<RackDto>(rack);
        }
    }
}