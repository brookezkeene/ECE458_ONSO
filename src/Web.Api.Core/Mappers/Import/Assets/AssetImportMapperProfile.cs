using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Web.Api.Common.Extensions;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Mappers.Import.Assets
{
    public class AssetImportMapperProfile : Profile
    {
        public AssetImportMapperProfile()
        {
            CreateMap<ImportAssetDto, AssetDto>()
                .ForMember(o => o.Id, opts => opts.MapFrom<ImportAssetLookupResolver>())
                .ForMember(o => o.ModelId, opts => opts.MapFrom<ImportAssetModelLookupResolver>())
                .ForMember(o => o.RackId, opts => opts.MapFrom<ImportAssetRackLookupResolver>())
                .ForMember(o => o.OwnerId, opts => opts.MapFrom<ImportAssetUserLookupResolver>())
                .ForMember(o => o.Model, opts => opts.Ignore())
                .ForMember(o => o.Rack, opts => opts.Ignore())
                .ForMember(o => o.Owner, opts => opts.Ignore())
                .ForMember(o => o.NetworkPorts, opts => opts.Ignore())
                .ForMember(o => o.PowerPorts, opts => opts.Ignore())
                .ForMember(o => o.LastUpdatedDate, opts => opts.MapFrom(src => DateTime.Now))
                .AfterMap<HydrateAssetPowerPorts>();
        }
    }

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