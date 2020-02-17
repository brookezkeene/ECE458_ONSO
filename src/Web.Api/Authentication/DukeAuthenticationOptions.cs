
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using static AspNet.Security.OAuth.Duke.DukeAuthenticationConstants;

namespace AspNet.Security.OAuth.Duke
{
    /// <summary>
    /// Defines a set of options used by <see cref="DukeAuthenticationHandler"/>.
    /// </summary>
    public class DukeAuthenticationOptions : OAuthOptions
    {
        public DukeAuthenticationOptions()
        {
            ClaimsIssuer = DukeAuthenticationDefaults.Issuer;

            CallbackPath = DukeAuthenticationDefaults.CallbackPath;

            AuthorizationEndpoint = DukeAuthenticationDefaults.AuthorizationEndpoint;
            TokenEndpoint = DukeAuthenticationDefaults.TokenEndpoint;
            UserInformationEndpoint = DukeAuthenticationDefaults.UserInformationEndpoint;

            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "firstName");
            ClaimActions.MapJsonKey(ClaimTypes.Name, "displayName");
            ClaimActions.MapJsonKey(ClaimTypes.Email, "mail");
            ClaimActions.MapJsonKey(Claims.Name, "netid");

        }

    }
}