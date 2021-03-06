﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace AspNet.Security.OAuth.Duke
{
    /// <summary>
    /// Default values used by the Duke authentication middleware.
    /// </summary>
    public static class DukeAuthenticationDefaults
    {
        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.Name"/>.
        /// </summary>
        public const string AuthenticationScheme = "Duke";

        /// <summary>
        /// Default value for <see cref="AuthenticationScheme.DisplayName"/>.
        /// </summary>
        public const string DisplayName = "Duke";

        /// <summary>
        /// Default value for <see cref="AuthenticationSchemeOptions.ClaimsIssuer"/>.
        /// </summary>
        public const string Issuer = "Duke";

        /// <summary>
        /// Default value for <see cref="RemoteAuthenticationOptions.CallbackPath"/>.
        /// </summary>
        public const string CallbackPath = "/signin-duke";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.AuthorizationEndpoint"/>.
        /// </summary>
        public const string AuthorizationEndpoint = "https://oauth.oit.duke.edu/oauth/authorize.php";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.TokenEndpoint"/>.
        /// </summary>
        public const string TokenEndpoint = "https://oauth.oit.duke.edu/oauth/token.php";

        /// <summary>
        /// Default value for <see cref="OAuthOptions.UserInformationEndpoint"/>.
        /// </summary>
        public const string UserInformationEndpoint = "https://oauth.oit.duke.edu/oauth/resource.php";
      
    }
}