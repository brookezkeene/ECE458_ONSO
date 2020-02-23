using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common;
using Web.Api.Dtos;
using Web.Api.Dtos.AuditEvents;

namespace Web.Api.Controllers
{
    [Route("api/audit-logs")]
    [ApiController]
    public class AuditLogsController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedList<GetAuditEventApiDto>>> GetMany(string searchText, int page = 1,
            int pageSize = 10)
        {
            return Ok(null);
        }
    }
}