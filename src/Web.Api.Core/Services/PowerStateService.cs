using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Web.Api.Core.Dtos;
using Web.Api.Core.Dtos.Power;
using Web.Api.Core.Services.Interfaces;

namespace Web.Api.Core.Services
{
    public class PowerStateService
    {
        private static Regex _regex;
        private const string Pattern = @"^.*>(\d+)<.*>(ON|OFF)<";
        private readonly IAssetService _assetService;

        public HttpClient Client { get; }

        public PowerStateService(HttpClient client, IAssetService assetService)
        {
            _regex = new Regex(Pattern, RegexOptions.Compiled | RegexOptions.Multiline);
            _assetService = assetService;

            // TODO: REMEMBER TO CHANGE THIS BACK TO OUR GROUP PORT 8003
            client.BaseAddress = new Uri("http://hyposoft-mgt.colab.duke.edu:8000");

            Client = client;

        }

        public async Task<AssetPowerStateDto> GetPowerStateAsync(Guid assetId)
        {
            var asset = await _assetService.GetAssetAsync(assetId);
            var pduPorts = asset.PowerPorts
                .Where(o => o.PduPort != null)
                .GroupBy(o => o.PduPort.Pdu.ToString())
                .ToDictionary(d => d.Key,
                    d => d.Select(o => o.PduPort.Number)
                        .ToList());

            var powerStates = new List<AssetPowerPortStateDto>();

            foreach (var kvp in pduPorts)
            {
                var response = await Client.GetAsync(
                    $"/pdu.php?pdu={kvp.Key}");
                response.EnsureSuccessStatusCode();
                var parsedResponse = ParseResponse(await response.Content.ReadAsStringAsync());
                var states = kvp.Value.Select(n => new AssetPowerPortStateDto()
                {
                    Port = kvp.Key + n,
                    Status = (PowerState)Enum.Parse(typeof(PowerState), parsedResponse[n], true)
                });
                powerStates.AddRange(states);
            }
            var ret = new AssetPowerStateDto()
            {
                Id = assetId,
                PowerPorts = powerStates
            };

            return ret;
        }

        public async Task<AssetPowerStateDto> UpdatePowerStateAsync(Guid assetId, PowerAction state)
        {

            var pduPorts = (await _assetService.GetAssetAsync(assetId))
                .PowerPorts
                .Where(o => o.PduPort != null)
                .GroupBy(o => o.PduPort.Pdu.ToString())
                .ToDictionary(d => d.Key,
                    d => d.Select(o => o.PduPort.Number)
                        .ToList());
            var powerStates = new List<AssetPowerPortStateDto>();

            foreach (var kvp in pduPorts)
            {
                // data is pdu: (hpdu-rtp1-A01L) (rack address + pdunumber + (LorR)
                // port: number
                // v: on, off
                foreach (var port in kvp.Value)
                {
                    var requestData = new Dictionary<String, String>
                    {
                        { "pdu", kvp.Key },
                        { "port", port.ToString() },
                        { "v", state.ToString().ToLower() }
                    };

                    if (state.ToString().ToLower().Equals("cycle"))
                    {
                        requestData.Remove("v");
                        requestData.Add("v", "off");
                        var d = new FormUrlEncodedContent(requestData);
                        var r = await Client.PostAsync("/power.php", d);
                        await Task.Delay(2000);

                        // Two seconds after sending the power off signal, send power on
                        requestData.Remove("v");
                        requestData.Add("v", "on");
                    } 

                    var data = new FormUrlEncodedContent(requestData);
                    var response = await Client.PostAsync("/power.php", data);
                    Console.WriteLine(response);

                    // Translate the action that was taken on the asset into a current power state
                    var powerState = PowerState.Off;
                    if (state == PowerAction.On || state == PowerAction.Cycle) {
                        powerState = PowerState.On;
                    }

                    var powerPortState = new AssetPowerPortStateDto()
                    {
                        Port = kvp.Key.ToString(),
                        Status = powerState
                    };

                    powerStates.Add(powerPortState);
                }
            }
            var ret = new AssetPowerStateDto()
            {
                Id = assetId,
                PowerPorts = powerStates
            };
            return ret;
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