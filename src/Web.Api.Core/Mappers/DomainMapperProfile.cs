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
                .ForMember(o => o.RowLetter, opts => opts.MapFrom(src => src.Row))
                .ForMember(o => o.RackNumber, opts => opts.MapFrom(src => src.Column));

            CreateMap<User, FlatUserDto>()
                .ReverseMap();
            CreateMap<RegisterUserDto, User>();

            // flat dtos
            CreateMap<Instance, FlatInstanceDto>()
                .ReverseMap();
            // also need to go from full -> flat dto
            CreateMap<Model, FlatModelDto>()
                .ReverseMap();
            CreateMap<Rack, FlatRackDto>()
                .ForMember(o => o.RowLetter, opts => opts.MapFrom(src => src.Row))
                .ForMember(o => o.RackNumber, opts => opts.MapFrom(src => src.Column));
        }
    }
}