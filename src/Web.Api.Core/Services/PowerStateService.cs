using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Web.Api.Core.Dtos;
using Web.Api.Core.Dtos.Power;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Entities;

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

            client.BaseAddress = new Uri("http://hyposoft-mgt.colab.duke.edu:8003");

            Client = client;

        }

        public async Task<AssetPowerStateDto> GetPowerStateAsync(Guid assetId)
        {
            var asset = await _assetService.GetAssetAsync(assetId);
            if (asset.Model.MountType == "blade") return GetBladePowerStatus(asset); 
            
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
            var asset = await _assetService.GetAssetAsync(assetId);
            if (asset.Model.MountType == "blade") return await SetBladePowerStatus(asset, state);

            var pduPorts = asset.PowerPorts
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
            //GetChassisPower();
            return ret;
        }

        private static IDictionary<int, string> ParseResponse(string response)
        {
            var matches = _regex.Matches(response);
            return matches.ToDictionary(match => int.Parse(match.Groups[1]
                .Value), match => match.Groups[2]
                .Value);
        }

        private static AssetPowerStateDto GetBladePowerStatus(AssetDto blade)
        {
            var response = $"expect BCMAN.expect {blade.Chassis.Hostname} {blade.ChassisSlot}".Execute();
            //Console.WriteLine($"BCMAN response: {response}");
            var regex = new Regex($"OK: chassis '{blade.Chassis.Hostname}' blade {blade.ChassisSlot} is (ON|OFF)", RegexOptions.Multiline);
            var match = regex.Match(response);
            foreach (var g in match.Groups)
            {
                Console.WriteLine($"match: {g}");
            }
            var status = match.Groups.Count >= 2
                ? match.Groups[1]
                    .Value
                : "ERROR";

            return new AssetPowerStateDto()
            {
                Id = blade.Id,
                PowerPorts = new List<AssetPowerPortStateDto>()
                {
                    new AssetPowerPortStateDto()
                    {
                        Port = "_",
                        Status = MapBcmanStatusToPowerState(status)
                    }
                }
            };
        }

        private static PowerState MapBcmanStatusToPowerState(string state)
        {
            return state switch
            {
                "ON" => PowerState.On,
                _ => PowerState.Off
            };
        }

        private static async Task<AssetPowerStateDto> SetBladePowerStatus(AssetDto blade, PowerAction action)
        {
            if (action == PowerAction.Cycle)
            {
                await SetBladePowerStatus(blade, PowerAction.Off);
                await Task.Delay(2000);
                action = PowerAction.On;
            }

            var response = $"expect BCMAN.expect {blade.Chassis.Hostname} {blade.ChassisSlot} {action}".Execute();

            return GetBladePowerStatus(blade);
        }
    }

    public static class BashExtensions
    {
        public static string Execute(this string cmd)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return string.Empty; // fuck you microsoft. i liked this language before this project. not anymore.

            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            return result;
        }
    }
}