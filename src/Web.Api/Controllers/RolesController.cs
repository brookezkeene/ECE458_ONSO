using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<PagedList<IdentityRole>>> GetMany(string searchText, int page = 1,
            int pageSize = 10)
        {
            return Ok(null);
        }
    }
}