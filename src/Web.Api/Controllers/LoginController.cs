using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IIdentityService _identityService;

        public LoginController(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDto login)
        {
            try
            {
                var token = await _identityService.LoginAsync(login);
                return Ok(token);
            }
            catch (InvalidCredentialException ex)
            {
                return Problem(ex.Message, statusCode: 401);
            }

        }

        [HttpPost("test")]
        public IActionResult ExternalLogin()
        {
            // Note: the authenticationScheme parameter must match the value configured in Startup.cs
            return Challenge(new AuthenticationProperties() { RedirectUri = "./external" }, "Duke");
        }

        [HttpGet("external")]
        public IActionResult ExternalLoginCallback(string returnUrl)
        {
            try
            {
                //here get the credentials of the user
                //do i need a token here?
                return Challenge(new AuthenticationProperties() { RedirectUri = "./" }, "Duke");
            }
            catch (InvalidCredentialException ex)
            {
                return Problem(ex.Message, statusCode: 401);
            } 
        }
    }
}