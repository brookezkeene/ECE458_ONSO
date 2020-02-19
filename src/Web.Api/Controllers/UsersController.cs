using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public UsersController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpGet]
        public async Task<ActionResult<PagedList<UserDto>>> Get(string searchText, int page = 1, int pageSize = 10)
        {
            var users = await _identityService.GetUsersAsync(searchText, page, pageSize);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(Guid id)
        {
            var user = await _identityService.GetUserAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Post([FromBody] RegisterUserDto user)
        {
            var (identityResult, userId) = await _identityService.CreateUserAsync(user);
            var createdUser = await _identityService.GetUserAsync(userId);
            return CreatedAtAction(nameof(Get), new {id = createdUser.Id}, createdUser);
        }

        [HttpGet("{id}/roles")]
<<<<<<< HEAD
        public async Task<ActionResult<PagedList<GetUserRolesApiDto>>> GetUserRoles(Guid id)
=======
        public async Task<ActionResult<PagedList<IdentityRole>>> GetUserRoles(Guid userId)
>>>>>>> add roles endpoints
        {
            return Ok(null);
        }

        [HttpPost("{id}/roles")]
<<<<<<< HEAD
        public async Task<IActionResult> PostUserRoles(Guid id, [FromBody] CreateUserRolesApiDto roles)
=======
        public async Task<IActionResult> PostUserRoles(Guid userId, [FromBody] CreateUserRolesApiDto roles)
>>>>>>> add roles endpoints
        {
            return Ok();
        }
    }
}