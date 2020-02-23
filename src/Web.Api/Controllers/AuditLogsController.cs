using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public AuditLogsController(IAuditLogService auditLogService)
        {
            _auditLogService = auditLogService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<GetAuditEventApiDto>>> GetMany(string searchText, int page = 1,
            int pageSize = 10)
        {
            var events = await _auditLogService.GetAsync(searchText, page, pageSize);
            var response = events.MapTo<PagedList<GetAuditEventApiDto>>();
            return Ok(response);
        }
    }
}