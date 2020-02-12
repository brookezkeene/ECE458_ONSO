using System;
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
                .ForMember(x => x.Rack, opts => opts.MapFrom(src => $"{src.Rack.Row}{src.Rack.Column}"));
            CreateMap<InstanceDto, Instance >()
                .ForPath(x => x.Rack.Row, opts => opts.MapFrom(src => src.Rack[0]))
                .ForPath(x => x.Rack.Column, opts => opts.MapFrom(src => int.Parse(src.Rack.Substring(1))));


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

            // need to get from full -> export ready DTOs for models and instances 
            CreateMap<Model, ExportModelDto>()
                .ForMember(o => o.vendor, opts => opts.MapFrom(src => src.Vendor))
                .ForMember(o => o.model_number, opts => opts.MapFrom(src => src.ModelNumber))
                .ForMember(o => o.height, opts => opts.MapFrom(src => src.Height))
                .ForMember(o => o.display_color, opts => opts.MapFrom(src => src.DisplayColor))
                .ForMember(o => o.ethernet_ports, opts => opts.MapFrom(src => src.EthernetPorts))
                .ForMember(o => o.power_ports, opts => opts.MapFrom(src => src.PowerPorts))
                .ForMember(o => o.cpu, opts => opts.MapFrom(src => src.Cpu))
                .ForMember(o => o.memory, opts => opts.MapFrom(src => src.Memory))
                .ForMember(o => o.storage, opts => opts.MapFrom(src => src.Storage))
                .ForMember(o => o.comment, opts => opts.MapFrom(src => src.Comment));
            CreateMap<Instance, ExportInstanceDto>()
                .ForMember(o => o.hostname, opts => opts.MapFrom(src => src.Hostname))
                .ForMember(o => o.rack, opts => opts.MapFrom(src => $"{src.Rack.Row}{src.Rack.Column}"))
                .ForMember(o => o.rack_position, opts => opts.MapFrom(src => src.RackPosition))
                .ForMember(o => o.vendor, opts => opts.MapFrom(src => src.Model.Vendor))
                .ForMember(o => o.model_number, opts => opts.MapFrom(src => src.Model.ModelNumber))
                .ForMember(o => o.owner, opts => opts.MapFrom(src => src.Owner.UserName))
                .ForMember(o => o.comment, opts => opts.MapFrom(src => src.Comment));

        }
    }
}