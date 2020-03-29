using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Mappers.Import.Models
{
    public class HydrateModelAction : IMappingAction<ImportModelDto, ModelDto>
    {
        private readonly IModelRepository _modelRepository;
        private readonly IMapper _mapper;

        public HydrateModelAction(IModelRepository modelRepository, IMapper mapper)
        {
            _modelRepository = modelRepository;
            _mapper = mapper;
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
                    .Select(o =>
                        new
                            ModelNetworkPort // this prevents auto-mapper from attempting to lazy load assets when mapping back to the dto
                            {
                                Id = o.Id,
                                Number = o.Number,
                                Name = o.Name
                            })
                    .ToList();
                var dtos = _mapper.Map<List<ModelNetworkPortDto>>(otherNetworkPorts);
                destination.NetworkPorts.AddRange(dtos);
            }
        }
    }
}