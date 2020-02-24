using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace AspNet.Security.OAuth.Duke
{
    public class DukeAuthenticationHandler : OAuthHandler<DukeAuthenticationOptions>
    {
        public DukeAuthenticationHandler(
            [NotNull] IOptionsMonitor<DukeAuthenticationOptions> options,
            [NotNull] ILoggerFactory logger,
            [NotNull] UrlEncoder encoder,
            [NotNull] ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticationTicket> CreateTicketAsync(
            [NotNull] ClaimsIdentity identity,
            [NotNull] AuthenticationProperties properties,
            [NotNull] OAuthTokenResponse tokens)
        {
            //using var request = new HttpRequestMessage(HttpMethod.Get, Options.UserInformationEndpoint);
            using var request = new HttpRequestMessage(HttpMethod.Get, "https://api.colab.duke.edu/identity/v1/");
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.Add("x-api-key", "determined-shannon");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            using var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, Context.RequestAborted);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogError("An error occurred while retrieving the user profile: the remote server " +
                                "returned a {Status} response with the following payload: {Headers} {Body}.",
                                /* Status: */ response.StatusCode,
                                /* Headers: */ response.Headers.ToString(),
                                /* Body: */ await response.Content.ReadAsStringAsync());

                throw new HttpRequestException("An error occurred while retrieving the user profile.");
            }

            var payload = await response.Content.ReadAsStringAsync();

            using var jsonDocument = JsonDocument.Parse(payload);
            var principal = new ClaimsPrincipal(identity);
            var context = new OAuthCreatingTicketContext(principal, properties, Context, Scheme, Options, Backchannel, tokens, jsonDocument.RootElement);
            context.RunClaimActions();

            var dukeInfo = JsonConvert.DeserializeObject<DukeOAthInfo>(payload);
            identity.AddClaim(new Claim(ClaimTypes.Email, dukeInfo.mail));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, dukeInfo.firstName));
            identity.AddClaim(new Claim(ClaimTypes.Surname, dukeInfo.lastName));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, dukeInfo.netid));

            await Options.Events.CreatingTicket(context);
            var what = new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
            return what;
        }
    }
    public class LDAP
    {
        public string dn { get; set; }
        public string key { get; set; }
        public List<string> objectClass { get; set; }
        public string duSponsor { get; set; }
    }

    public class Settings
    {
        public string homeDirectory { get; set; }
        public string remoteHomeDirectory { get; set; }
        public string loginShell { get; set; }
        public string uidNumber { get; set; }
        public string gidNumber { get; set; }
    }

    public class DukeOAthInfo
    {
        public string duPSAcadCareerC1 { get; set; }
        public string duPSAcadCareerDescC1 { get; set; }
        public string duPSAcadProgC1 { get; set; }
        public string duDukeidHistory { get; set; }
        public string duDukeID { get; set; }
        public string eduPersonPrimaryAffiliation { get; set; }
        public string displayName { get; set; }
        public string duMiddleName1 { get; set; }
        public string eduPersonPrincipalName { get; set; }
        public string mail { get; set; }
        public string duSAPOrgUnit { get; set; }
        public string duSAPCompany { get; set; }
        public string duSAPCompanyDesc { get; set; }
        public string netid { get; set; }
        public string lastName { get; set; }
        public string firstName { get; set; }
        public string nickname { get; set; }
        public string gradYear { get; set; }
        public LDAP LDAP { get; set; }
        public Settings settings { get; set; }
    }
}