using System;
using System.Linq;
using AutoMapper;
using Microsoft.VisualBasic;
using Web.Api.Common;
using Web.Api.Common.Mappers;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Mappers
{
    public class DomainMapperProfile : PaginatedProfile
    {
        public DomainMapperProfile()
        {
            // full dtos w/ nested dtos
            CreateMap<Model, ModelDto>()
                .ForMember(o => o.Assets, opts => opts.MapFrom(src => src.Assets))
                .ForMember(o => o.NetworkPorts, opts => opts.MapFrom(src => src.NetworkPorts))
                .ReverseMap();
            CreateMap<ModelNetworkPort, ModelNetworkPortDto>()
                .ForMember(o => o.Model, opts => opts.MapFrom(src => src.Model))
                .ReverseMap();

            CreateMap<Asset, AssetDto>()
                .ReverseMap();
            CreateMap<AssetPowerPort, AssetPowerPortDto>()
                .ReverseMap();
            CreateMap<AssetNetworkPort, AssetNetworkPortDto>()
                .ReverseMap();
            CreateMap<PduPort, PduPortDto>()
                .ReverseMap();
            CreateMap<Pdu, PduDto>()
                .ReverseMap();

            CreateMap<Datacenter, DatacenterDto>()
                .ReverseMap();

            CreateMap<Rack, RackDto>()
                .ForMember(o => o.RowLetter, opts => opts.MapFrom(src => src.Row))
                .ForMember(o => o.RackNumber, opts => opts.MapFrom(src => src.Column))
                .ForMember(o => o.Datacenter, opts => opts.MapFrom(src => src.Datacenter))
                .ReverseMap();

            CreateMap<User, UserDto>()
                .ReverseMap();
            CreateMap<RegisterUserDto, User>(MemberList.Source)
                .ForSourceMember(o => o.Password, opts => opts.DoNotValidate())
                .ForSourceMember(o => o.Role, opts => opts.DoNotValidate());

            CreateMap<PagedList<Asset>, PagedList<AssetDto>>();
        }
    }
}