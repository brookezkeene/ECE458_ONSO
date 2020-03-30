using System.Collections.Generic;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;

namespace Web.Api.Core.Mappers.Import.Assets
{
    public class ImportNetworkConnectionConverter : ITypeConverter<ImportNetworkConnectionDto, NetworkConnectionDto>
    {
        private readonly INetworkService _networkService;

        public ImportNetworkConnectionConverter(INetworkService networkService)
        {
            _networkService = networkService;
        }

        public NetworkConnectionDto Convert(ImportNetworkConnectionDto source, NetworkConnectionDto destination,
            ResolutionContext context)
        {
            destination ??= new NetworkConnectionDto();

            var sourcePort = _networkService.GetNetworkPort(source.SourceHostname, source.SourcePortName);
            var destPort = _networkService.GetNetworkPort(source.DestinationHostname, source.DestinationPortName);

            // overwrite mac address for the source only, as no mac address is uploaded for the destination on each import row
            sourcePort.MacAddress = source.SourceMacAddress;

            destination.Ports = new List<AssetNetworkPortDto>
            {
                sourcePort,
                destPort
            };

            return destination;
        }
    }
}