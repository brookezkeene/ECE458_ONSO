using System.Collections.Generic;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Mappers
{
    public class CreateNetworkConnection : IMappingAction<AssetNetworkPortDto, AssetNetworkPort>
    {
        private readonly INetworkRepository _repo;

        public CreateNetworkConnection(INetworkRepository repo)
        {
            _repo = repo;
        }

        public void Process(AssetNetworkPortDto source, AssetNetworkPort destination, ResolutionContext context)
        {
            if (!source.ConnectedPortId.HasValue) return;

            destination.NetworkConnection = new NetworkConnection
            {
                Ports = new List<AssetNetworkPort>
                {
                    destination,
                    _repo.GetNetworkPort(source.ConnectedPortId.Value)
                }
            };
        }
    }
}