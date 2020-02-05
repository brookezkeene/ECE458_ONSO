using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;

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
        public async Task<ActionResult<PagedList<FlatUserDto>>> Get(string searchText, int page = 1, int pageSize = 10)
        {
            var users = await _identityService.GetUsersAsync(searchText, page, pageSize);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlatUserDto>> Get(Guid id)
        {
            var user = await _identityService.GetUserAsync(id);
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<FlatUserDto>> Post([FromBody] RegisterUserDto user)
        {
            Console.WriteLine($"Received registration request from ${user.Username}");
            var (identityResult, userId) = await _identityService.CreateUserAsync(user);
            var createdUser = await _identityService.GetUserAsync(userId);
            return CreatedAtAction(nameof(Get), new {id = createdUser.Id}, createdUser);
        }
    }
}