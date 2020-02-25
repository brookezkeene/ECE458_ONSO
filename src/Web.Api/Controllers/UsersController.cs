using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Dtos;
using Web.Api.Dtos.Users;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        public UsersController(IIdentityService identityService, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager)
        {
            _identityService = identityService;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
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
            return CreatedAtAction(nameof(Get), new { id = createdUser.Id }, createdUser);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDto>> Delete(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound("User not found.");
            } else if (user.FirstName == "Admin")
            {
                return Forbid();
            }

            return Ok(await _identityService.DeleteUserAsync(id));
        }

        [HttpGet("{id}/roles")]
        public async Task<ActionResult<List<string>>> GetUserRoles(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound("User not found.");
            }
            return Ok(await _userManager.GetRolesAsync(user));
        }

        [HttpPost("{id}/roles")]
        public async Task<IActionResult> PostUserRoles(Guid id, [FromBody] UpdateUserRoleApiDto role)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound("User not found.");
            } else if (user.FirstName == "Admin")
            {
                return Forbid();
            }

            var otherRole = "basic";
            if (role.Name == "basic")
            {
                otherRole = "admin";
            }
            await _userManager.RemoveFromRoleAsync(user, otherRole);

            var roleExists = await _roleManager.RoleExistsAsync(role.Name);
            if (!roleExists)
            {
                return NotFound("Role not found.");
            }

            if (!await _userManager.IsInRoleAsync(user, role.Name))
            {
                return Ok(await _userManager.AddToRoleAsync(user, role.Name));
            } else
            {
                return BadRequest("User exists in Role.");
            }
        }

        [HttpGet("me")]
        public ActionResult<IEnumerable<object>> GetMyInfo()
        {
            if (!HttpContext.User.Identity.IsAuthenticated) return Ok();
            var claims = HttpContext.User.Claims.Select(o => new {o.Type, o.Value});
            
            return Ok(claims);
        }
    }
}