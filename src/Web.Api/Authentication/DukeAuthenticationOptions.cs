using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

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

            SaveTokens = true;

            ClaimActions.MapJsonKey(ClaimTypes.Email, "mail");
            ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "netid");
            ClaimActions.MapJsonKey(ClaimTypes.GivenName, "firstName");
            ClaimActions.MapJsonKey(ClaimTypes.Surname, "lastName");
        }

    }
}