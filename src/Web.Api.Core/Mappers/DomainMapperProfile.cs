using System.Linq;
using AutoMapper;
using Microsoft.VisualBasic;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Mappers
{
    public class DomainMapperProfile : Profile
    {
        public DomainMapperProfile()
        {
            // full dtos w/ nested dtos
            CreateMap<Model, ModelDto>()
                .ReverseMap();
            CreateMap<Instance, InstanceDto>()
                .ForMember(x => x.Rack, opts => opts.MapFrom(src => $"{src.Rack.Row}{src.Rack.Column}"))
                .ReverseMap();
            CreateMap<Rack, RackDto>()
                .ReverseMap();
            CreateMap<User, FlatUserDto>()
                .ReverseMap();

            // flat dtos
            CreateMap<Instance, FlatInstanceDto>();
            // also need to go from full -> flat dto
            CreateMap<Model, FlatModelDto>();
            CreateMap<Rack, FlatRackDto>();
        }
    }
}