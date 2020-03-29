using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Web.Api.Controllers;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services.Interfaces;

namespace Web.Api.Mappers
{
    public class PowerConnectionApiMapperProfile : Profile
    {
        public PowerConnectionApiMapperProfile()
        {
            CreateMap<CreatePowerConnectionApiDto, PowerConnectionDto>()
                .ConvertUsing<ConvertCreatePowerConnectionApiDtoToPowerConnectionDto>();
        }
    }
}
