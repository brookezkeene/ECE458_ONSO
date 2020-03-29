using System.Collections.Generic;
using AutoMapper;
using Web.Api.Controllers;
using Web.Api.Core.Dtos;

namespace Web.Api.Mappers
{
    public class ConvertCreatePowerConnectionApiDtoToPowerConnectionDto : ITypeConverter<CreatePowerConnectionApiDto, PowerConnectionDto>
    {
        public PowerConnectionDto Convert(CreatePowerConnectionApiDto source, PowerConnectionDto destination,
            ResolutionContext context)
        {
            destination ??= new PowerConnectionDto();
            destination.Ports = new List<PowerPortDto>
            {
                new AssetPowerPortDto(source.AssetPowerPortId),
                new PduPortDto(source.PduPortId)
            };

            return destination;
        }
    }
}