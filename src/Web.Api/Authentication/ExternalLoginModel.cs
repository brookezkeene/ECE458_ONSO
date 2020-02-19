using AspNet.Security.OAuth.Duke;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Authentication
{
    //TODO: does this have to be a pagemodel
    //TODO: where does the signinManager and UserManager come from
    public class ExternalLoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ExternalLoginModel> _logger;

        public ExternalLoginModel(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<ExternalLoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        public string LoginProvider { get; set; }

        //TODO: do i need the remoteError input string?
        public async Task<IActionResult> OnGetCallbackAsync()
        {
            //gets the info of the current login page: duke
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToPage("./");
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

                _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);
                //TODO: should I do anything special here
                return RedirectToPage("./Models");
            }

            else
            {
                //adding the new user into the database 
                var user = new User { };
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
                {
                    user.Email = info.Principal.FindFirstValue(ClaimTypes.Email);
                }
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
                {
                    user.UserName = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                }
                if (info.Principal.HasClaim(c => c.Type == ClaimTypes.GivenName))
                {
                    user.FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
                }
                if(info.Principal.HasClaim(c=> c.Type == ClaimTypes.Surname))
                {
                    user.LastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
                }
               
                //putting the new user into the database 
                var addUser = await _userManager.CreateAsync(user);
                if (addUser.Succeeded)
                {
                    addUser = await _userManager.AddLoginAsync(user, info);
                    if (addUser.Succeeded)
                    {

                        // Include the access token in the properties
                        var props = new AuthenticationProperties();
                        props.StoreTokens(info.AuthenticationTokens);

                        await _signInManager.SignInAsync(user, props, authenticationMethod: info.LoginProvider);
                        _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
                        return RedirectToPage("./");
                    }
                }

                return RedirectToPage("./");
            }
        }
    }
}
