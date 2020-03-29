using System;
using System.Linq;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Mappers
{
    public class HydrateNetworkPortsFromModel : IMappingAction<AssetDto, Asset>
    {
        private readonly IModelRepository _modelRepository;

        public HydrateNetworkPortsFromModel(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public void Process(AssetDto source, Asset destination, ResolutionContext context)
        {
            // do not hydrate if asset already exists
            if (destination.Id != default) return;

            // do not hydrate if asset already has the appropriate network ports from the model
            var model = _modelRepository.GetModel(destination.ModelId);
            if (model == null || destination.NetworkPorts.Count == model.NetworkPorts.Count) return;

            // This will cause any network connections to be lost if an asset manages to enter a state where
            // it is missing some of the network ports of its model. This state should never be reached.
            var networkPorts = model.NetworkPorts.Select(port => new AssetNetworkPort
                {
                    ModelNetworkPort = port,
                    ModelNetworkPortId = port.Id
                })
                .ToList();
            destination.NetworkPorts = networkPorts;
        }
    }
}