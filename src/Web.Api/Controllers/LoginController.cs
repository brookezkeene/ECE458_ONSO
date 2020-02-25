using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Api.Authentication;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public LoginController(IIdentityService identityService, SignInManager<User> signInManager,
            UserManager<User> userManager)
        {
            _identityService = identityService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginDto login)
        {
            var result = await _signInManager.PasswordSignInAsync(login.Username, login.Password, false, false);
            if (result.Succeeded)
            {
                return Ok();
            }

            return Problem("Invalid login attempt.", statusCode: 401);
        }

        [HttpPost("external")]
        public IActionResult ExternalLogin()
        {
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Duke", "/api/login/external");
            return new ChallengeResult("Duke", properties);
        }

        [HttpGet("external")]
        public async Task<ActionResult> ExternalLoginCallback()
        {
            try
            {
                //gets the info of the current login page: duke
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return Redirect("/");
                }

                // Sign in the user with this external login provider if the user already has a login.
                var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

                if (result.Succeeded)
                {
                    // Store the access token and resign in so the token is included in the cookie
                    var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
                    var props = new AuthenticationProperties();

                    props.StoreTokens(info.AuthenticationTokens);
                    await _signInManager.SignInAsync(user, props, info.LoginProvider);

                    //TODO: should I do anything special here
                    return Redirect("/");
                }

                else
                {
                    //adding the new user into the database 
                    var user = new User { };
                    var claims = new List<Claim>();
                    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                    {
                        claims.Add(info.Principal.FindFirst(ClaimTypes.Email));
                        user.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
                    }
                    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
                    {
                        // OAuth providers naturally use a NameIdentifier with a different value than what we'd like to use---the AspNetUser.Id GUID
                        // for this reason, we refrain from adding this claim manually, because ASP.NET Identity will set it for us 
                        user.UserName = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                    }
                    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.GivenName))
                    {
                        claims.Add(info.Principal.FindFirst(ClaimTypes.GivenName));
                        user.FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                    }
                    if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Surname))
                    {
                        claims.Add(info.Principal.FindFirst(ClaimTypes.Surname));
                        user.LastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
                    }

                    //putting the new user into the database 
                    var addUser = await _userManager.CreateAsync(user);
                    if (!addUser.Succeeded) return Redirect("/");

                    addUser = await _userManager.AddLoginAsync(user, info);
                    if (!addUser.Succeeded) return Redirect("/");

                    await _userManager.AddClaimsAsync(user, claims);
                    // Include the access token in the properties
                    var props = new AuthenticationProperties();
                    props.StoreTokens(info.AuthenticationTokens);
                    props.IsPersistent = true;

                    await _signInManager.SignInAsync(user, props, info.LoginProvider);
                    return Redirect("/");

                }
            }
            catch (InvalidCredentialException ex)
            {
                return Problem(ex.Message, statusCode: 401);
            } 
        }
    }
}