using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Core.Mappers
{
    public class PowerPortResolver : IValueResolver<PowerConnectionDto, PowerConnection, List<PowerPort>>
    {
        private readonly IPowerRepository _powerRepo;

        public PowerPortResolver(IPowerRepository powerRepo)
        {
            _powerRepo = powerRepo;
        }

        public List<PowerPort> Resolve(PowerConnectionDto source, PowerConnection destination, List<PowerPort> destMember, ResolutionContext context)
        {
            return source.Ports.Select<PowerPortDto, PowerPort>(powerPort =>
                {
                    // use the appropriate repository method for the given type of power port
                    return powerPort switch
                    {
                        AssetPowerPortDto _ => _powerRepo.GetAssetPowerPort(powerPort.Id),
                        PduPortDto _ => _powerRepo.GetPduPort(powerPort.Id),
                        _ => throw new ArgumentException($"Unsupported power port type: {powerPort.GetType()}", nameof(powerPort))
                    };
                })
                .ToList();
        }
    }
}