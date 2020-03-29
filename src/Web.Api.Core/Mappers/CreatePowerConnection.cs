using System.Collections.Generic;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Mappers
{
    public class CreatePowerConnection : IMappingAction<AssetPowerPortDto, AssetPowerPort>
    {
        private readonly IPowerRepository _repo;

        public CreatePowerConnection(IPowerRepository repo)
        {
            _repo = repo;
        }

        public void Process(AssetPowerPortDto source, AssetPowerPort destination, ResolutionContext context)
        {
            if (!source.PduPortId.HasValue) return;

            destination.PowerConnection = new PowerConnection
            {
                Ports = new List<PowerPort>
                {
                    destination,
                    _repo.GetPduPort(source.PduPortId.Value)
                }
            };
        }
    }
}