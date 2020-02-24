
using System;
using System.Diagnostics.CodeAnalysis;
using AspNet.Security.OAuth.Duke;
using Microsoft.AspNetCore.Authentication;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods to add Duke authentication capabilities to an HTTP application pipeline.
    /// </summary>
    public static class DukeAuthenticationExtensions
    {
        /// <summary>
        /// Adds <see cref="DukeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Duke authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddDuke([NotNull] this AuthenticationBuilder builder)
        {
            return builder.AddDuke(DukeAuthenticationDefaults.AuthenticationScheme, options => { });
        }

        /// <summary>
        /// Adds <see cref="DukeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Duke authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="configuration">The delegate used to configure the OpenID 2.0 options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddDuke(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] Action<DukeAuthenticationOptions> configuration)
        {
            return builder.AddDuke(DukeAuthenticationDefaults.AuthenticationScheme, configuration);
        }

        /// <summary>
        /// Adds <see cref="DukeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Duke authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Duke options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddDuke(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [NotNull] Action<DukeAuthenticationOptions> configuration)
        {
            return builder.AddDuke(scheme, DukeAuthenticationDefaults.DisplayName, configuration);
        }

        /// <summary>
        /// Adds <see cref="DukeAuthenticationHandler"/> to the specified
        /// <see cref="AuthenticationBuilder"/>, which enables Duke authentication capabilities.
        /// </summary>
        /// <param name="builder">The authentication builder.</param>
        /// <param name="scheme">The authentication scheme associated with this instance.</param>
        /// <param name="caption">The optional display name associated with this instance.</param>
        /// <param name="configuration">The delegate used to configure the Duke options.</param>
        /// <returns>The <see cref="AuthenticationBuilder"/>.</returns>
        public static AuthenticationBuilder AddDuke(
            [NotNull] this AuthenticationBuilder builder,
            [NotNull] string scheme,
            [MaybeNull] string caption,
            [NotNull] Action<DukeAuthenticationOptions> configuration)
        {
            return builder.AddOAuth<DukeAuthenticationOptions, DukeAuthenticationHandler>(scheme, caption, configuration);
        }
    }
}