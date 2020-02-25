using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Web.Api.Core.Dtos;
using Web.Api.Core.Dtos.Power;

namespace Web.Api.Core.Services
{
    class PowerService : IPowerService
{
        private static Regex _regex;
        private const string Pattern = @"^.*>(\d+)<.*>(ON|OFF)<";
        public IAssetService _assetService;

        public HttpClient Client { get; }

        public PowerService(HttpClient client, IAssetService assetService)
        {
            _regex = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.Multiline);
            _assetService = assetService;

            client.BaseAddress = new Uri("http://hyposoft-mgt.colab.duke.edu:8003");

            Client = client;

        }

        public async Task<AssetPowerStateDto> getStates(Guid assetId)
        {
            var pduPorts = (await _assetService.GetAssetAsync(assetId))
                .PowerPorts
                .Where(o => o.PduPort != null)
                .GroupBy(o => o.ToString())
                .ToDictionary(d => d.Key,
                    d => d.Select(o => o.PduPort.Number)
                          .ToList());

            foreach (var kvp in pduPorts)
            {
                var response = await Client.GetAsync(
                "/pdu.php?pdu={kvp.Key}");
                response.EnsureSuccessStatusCode();
                var parsedResponse = ParseResponse(await response.Content.ReadAsStringAsync());
                var statesICareAbout = kvp.Value.Select(i => parsedResponse[i]);
            }

            var ret = new AssetPowerStateDto();

            return statesICareAbout;
            
        }

        public async Task<AssetPowerStateDto> setState(Guid assetId, PowerState state)
        {

        }

        private static IDictionary<int, string> ParseResponse(string response)
        {
            var matches = _regex.Matches(response);
            return matches.ToDictionary(match => int.Parse(match.Groups[1]
                .Value), match => match.Groups[2]
                .Value);
        }
    }
}
