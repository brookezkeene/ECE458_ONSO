using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Mappers
{
    public class AssetNetworkPortResolver : IValueResolver<NetworkConnectionDto, NetworkConnection, List<AssetNetworkPort>>
    {
        private readonly INetworkRepository _networkRepo;

        public AssetNetworkPortResolver(INetworkRepository networkRepo)
        {
            _networkRepo = networkRepo;
        }

        public List<AssetNetworkPort> Resolve(NetworkConnectionDto source, NetworkConnection destination, List<AssetNetworkPort> destMember, ResolutionContext context)
        {
            return source.Ports
                .Select(o =>
                {
                    var port = _networkRepo.GetNetworkPort(o.Id);
                    // overwrite the mac address with what has been provided
                    port.MacAddress = o.MacAddress;
                    return port;
                })
                .ToList();
        }
    }
}