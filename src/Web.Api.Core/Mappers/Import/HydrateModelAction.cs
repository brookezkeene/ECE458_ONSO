using System.Linq;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Mappers.Import
{
    public class HydrateModelAction : IMappingAction<ImportModelDto, ModelDto>
    {
        private readonly IModelRepository _modelRepository;

        public HydrateModelAction(IModelRepository modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public void Process(ImportModelDto source, ModelDto destination, ResolutionContext context)
        {
            var model = _modelRepository.GetModel(destination.Vendor, destination.ModelNumber);
            if (model == null) return;

            destination.Id = model.Id;

            model.NetworkPorts.ForEach(port =>
            {
                var destPort = destination.NetworkPorts.FirstOrDefault(o => o.Number == port.Number);
                if (destPort != null)
                {
                    destPort.Id = port.Id;
                }
            });

            if (model.NetworkPorts.Count > 4)
            {
                var otherNetworkPorts = model.NetworkPorts
                    .OrderBy(o => o.Number)
                    .Skip(4)
                    .Select(o => new ModelNetworkPort // this prevents auto-mapper from attempting to lazy load assets when mapping back to the dto
                    {
                        Id = o.Id,
                        Number = o.Number,
                        Name = o.Name
                    })
                    .ToList()
                    .ToDto();
                destination.NetworkPorts.AddRange(otherNetworkPorts);
            }
        }
    }
}