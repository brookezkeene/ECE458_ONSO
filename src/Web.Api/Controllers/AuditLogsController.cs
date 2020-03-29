using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos;
using Web.Api.Dtos.AuditEvents;
using Web.Api.Mappers;

namespace Web.Api.Controllers
{
    [Route("api/audit-logs")]
    [ApiController]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditLogService _auditLogService;
        private readonly IMapper _mapper;

        public AuditLogsController(IAuditLogService auditLogService, IMapper mapper)
        {
            _auditLogService = auditLogService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<GetAuditLogApiDto>>> GetMany(string searchText, int page = 1,
            int pageSize = 10)
        {
            var events = await _auditLogService.GetAsync(searchText, page, pageSize);
            var response = _mapper.Map<PagedList<GetAuditLogApiDto>>(events);
            return Ok(response);
        }
    }
}