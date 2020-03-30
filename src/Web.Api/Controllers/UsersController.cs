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
        public async Task<ActionResult<List<string>>> GetUserRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(roles);
        }

        [HttpGet("{id}/claims")]
        public async Task<ActionResult<string>> GetUserClaims(Guid id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var claims = await _userManager.GetClaimsAsync(user);
            return Ok(claims);
        }

        [HttpPut("{id}/roles")]
        public async Task<IActionResult> PostUserRoles(string id, [FromBody] UpdateUserRoleApiDto roles)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.UserName == "admin")
            {
                return Forbid();
            }

            var requestedRolesExist = true;
            var containsAsset = false;
            foreach (var role in roles.Roles)
            {
                requestedRolesExist &= await _roleManager.RoleExistsAsync(role);
                // check if user's new permissions include assets
                if (role == "asset")
                {
                    containsAsset = true;
                }
            }
            if (!requestedRolesExist)
            {
                return NotFound("One or more of the requested roles do not exist.");
            }

            var allRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, allRoles);

            var add = await _userManager.AddToRolesAsync(user, roles.Roles);
            // add all datacenters to claims
            if (containsAsset)
            {
                var userClaims = await _userManager.GetClaimsAsync(user);
                if (userClaims != null)
                {
                    await _userManager.RemoveClaimsAsync(user, userClaims);
                }

                // default user to global datacenter permission
                if (roles.Datacenters == "" || roles.Datacenters.Contains("All Datacenters"))
                {
                    await _userManager.AddClaimAsync(user, new Claim("permission:datacenter", "All Datacenters"));
                }
                else
                {
                    await _userManager.AddClaimAsync(user, new Claim("permission:datacenter", roles.Datacenters));
                }
            }

            return Ok(add);
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