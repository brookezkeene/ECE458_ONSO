using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        private readonly SignInManager<User> _signInManager;

        public LogoutController(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}