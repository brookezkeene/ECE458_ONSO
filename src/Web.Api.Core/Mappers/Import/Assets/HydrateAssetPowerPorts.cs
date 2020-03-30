using System;
using System.Linq;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Mappers.Import.Assets
{
    public class HydrateAssetPowerPorts : IMappingAction<ImportAssetDto, AssetDto>
    {
        private readonly IPowerService _powerService;
        private readonly IModelRepository _modelRepo;

        public HydrateAssetPowerPorts(IPowerService powerService, IModelRepository modelRepo)
        {
            _powerService = powerService;
            _modelRepo = modelRepo;
        }

        public void Process(ImportAssetDto source, AssetDto destination, ResolutionContext context)
        {
            var maxPortNum = _modelRepo.GetModel(destination.ModelId).PowerPorts.GetValueOrDefault();

            var allPorts = destination.Id != default
                ? _powerService.GetAssetPowerPorts(destination.Id)
                : Enumerable.Range(1, maxPortNum)
                    .Select(n => new AssetPowerPortDto
                    {
                        Number = n
                    });

            // within this context we want a maximum of two ports, given that asset import supports up to 2 power connections
            var portsToImport = allPorts.Where(port => port.Number <= 2)
                .Select(port =>
                {
                    var connectedPduPortStr = source.GetType()
                        .GetProperty($"PowerPortConnection{port.Number}")
                        ?.GetValue(source) as string;

                    if (string.IsNullOrEmpty(connectedPduPortStr) || connectedPduPortStr.Length < 2) return port;

                    var locStr = connectedPduPortStr.Substring(0, 1).ToUpper();
                    var portNumStr = connectedPduPortStr.Substring(1);

                    var pduLocation = Enum.TryParse<PduLocation>(locStr, out var outPduLocation)
                        ? outPduLocation
                        : (PduLocation?) null;
                    var pduPortNumber = int.TryParse(portNumStr, out var outPduPortNumber)
                        ? outPduPortNumber
                        : (int?) null;

                    if (!pduLocation.HasValue || !pduPortNumber.HasValue) return port;

                    var pduPort = _powerService.GetPduPort(destination.RackId, pduLocation.Value, pduPortNumber.Value);
                    port.PduPort = pduPort;
                    port.PduPortId = pduPort?.Id;

                    return port;
                })
                .ToList();

            destination.PowerPorts = portsToImport;
        }
    }
}