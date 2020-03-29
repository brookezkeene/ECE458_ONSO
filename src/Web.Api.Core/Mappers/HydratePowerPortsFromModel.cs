using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Mappers
{
    public class HydratePowerPortsFromModel : IMappingAction<AssetDto, Asset>
    {
        private readonly IModelRepository _modelRepository;

        public HydratePowerPortsFromModel(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public void Process(AssetDto source, Asset destination, ResolutionContext context)
        {
            // do not hydrate if asset already exists
            if (destination.Id != default) return;

            // do not hydrate if asset already has the appropriate number of power ports
            var model = _modelRepository.GetModel(destination.ModelId);
            if (model == null || PortsNumberedCorrectly(model, destination.PowerPorts)) return;

            foreach (var (port, index) in destination.PowerPorts.Select((port, index) => ( port, index)))
            {
                port.Number = index + 1;
            }

            // This will cause any power connections to be lost if an asset manages to enter a state where
            // it is missing some of the power ports of its model. This state should never be reached.
            var powerPorts = Enumerable.Range(1, model.PowerPorts.GetValueOrDefault()) // full range from 1-# power ports
                .Except(destination.PowerPorts.Select(o => o.Number)) // exclude port #'s already mapped
                .Select(portNumber => new AssetPowerPort // create new power ports for those port #'s that still remain
                {
                    Number = portNumber
                })
                .Concat(destination.PowerPorts) // combine new power ports with previously mapped ports
                .ToList(); // done
            destination.PowerPorts = powerPorts;
        }

        private static bool PortsNumberedCorrectly(Model model, List<AssetPowerPort> ports)
        {
            return !Enumerable.Range(1, model.PowerPorts.GetValueOrDefault())
                .Except(ports.Select(port => port.Number))
                .Any();
        }
    }
}