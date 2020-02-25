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
using Newtonsoft.Json;

namespace Web.Api.Core.Services
{
    public class PowerService : IPowerService
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

            var powerStates = new List<AssetPowerPortStateDto>();

            foreach (var kvp in pduPorts)
            {
                var response = await Client.GetAsync(
                "/pdu.php?pdu={kvp.Key}");
                response.EnsureSuccessStatusCode();
                var parsedResponse = ParseResponse(await response.Content.ReadAsStringAsync());
                var statesICareAbout = kvp.Value.Select(i => parsedResponse[i]);
                var state = PowerState.Off;
                if(statesICareAbout.Equals("ON"))
                {
                    state = PowerState.On;
                }
                var powerPortState = new AssetPowerPortStateDto() {
                    Port = kvp.Key.ToString(),
                    Status = state
                };
                powerStates.Add(powerPortState);

        }
            var ret = new AssetPowerStateDto()
            {
                Id = assetId,
                PowerPorts = powerStates
            };

            return ret;
        }

        public async Task<AssetPowerStateDto> setStates(Guid assetId, PowerAction state)
        {

            var pduPorts = (await _assetService.GetAssetAsync(assetId))
                .PowerPorts
                .Where(o => o.PduPort != null)
                .GroupBy(o => o.ToString())
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

                    var requestData = new Data();
                    requestData.pdu = kvp.Key.ToString();
                    requestData.port = port.ToString();
                    requestData.v = state.ToString().ToLower();

                    var json = JsonConvert.SerializeObject(requestData);
                    var data = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await Client.PostAsync("/power.php", data);

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

public class Data {
    public string pdu { get; set; }
    public string port { get; set; }
    public string v { get; set; }
    }
