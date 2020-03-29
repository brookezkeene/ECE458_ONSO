using System;
using AutoMapper;
using Microsoft.VisualBasic;
using Web.Api.Common;
using Web.Api.Common.Mappers;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Entities.Extensions;

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
                .ReverseMap()
                .AfterMap<HydrateNetworkPortsFromModel>()
                .AfterMap<HydratePowerPortsFromModel>();

            CreateMap<AssetPowerPort, AssetPowerPortDto>()
                .ForMember(o => o.PduPort, opts => opts.MapFrom(src => src.PduPort()))
                .ForMember(o => o.PduPortId, opts => opts.MapFrom(src => src.PduPortId()))
                .ReverseMap()
                .AfterMap<CreatePowerConnection>();

            CreateMap<AssetNetworkPort, AssetNetworkPortDto>()
                .ForMember(o => o.ConnectedPortId, opts => opts.MapFrom(src => src.ConnectedPortId()))
                .ForMember(o => o.ConnectedPort, opts => opts.MapFrom(src => src.ConnectedPort()))
                .ReverseMap()
                .AfterMap<CreateNetworkConnection>();

            CreateMap<PduPort, PduPortDto>()
                .ForMember(o => o.AssetPowerPort, opts => opts.MapFrom(src => src.AssetPowerPort()))
                .ForMember(o => o.AssetPowerPortId, opts => opts.MapFrom(src => src.AssetPowerPortId()))
                .ReverseMap();
            CreateMap<Pdu, PduDto>()
                .ReverseMap();

            CreateMap<Datacenter, DatacenterDto>()
                .ReverseMap();
            CreateMap<DecommissionedAsset, DecommissionedAssetDto>()
                .ForMember(o => o.RackAddress, opts => opts.MapFrom(src => src.Rack))
                .ReverseMap();
            CreateMap<Rack, RackDto>()
                .ForMember(o => o.RowLetter, opts => opts.MapFrom(src => src.Row))
                .ForMember(o => o.RackNumber, opts => opts.MapFrom(src => src.Column))
                .ForMember(o => o.Datacenter, opts => opts.MapFrom(src => src.Datacenter))
                .ReverseMap();

            CreateMap<User, UserDto>()
                .ReverseMap();
            CreateMap<RegisterUserDto, User>(MemberList.Source)
                .ForSourceMember(o => o.Password, opts => opts.DoNotValidate());

            CreateMap<PagedList<Asset>, PagedList<AssetDto>>();

            CreateMap<ChangePlan, ChangePlanDto>()
                .ReverseMap();
            CreateMap<ChangePlanItem, ChangePlanItemDto>()
                .ReverseMap();

            CreateMap<NetworkConnectionDto, NetworkConnection>()
                .ForMember(o => o.Ports, opts => opts.MapFrom<AssetNetworkPortResolver>())
                .ReverseMap();

            CreateMap<PowerConnectionDto, PowerConnection>()
                .ForMember(o => o.Ports, opts => opts.MapFrom<PowerPortResolver>())
                .ReverseMap();

            CreateMap<PowerPort, PowerPortDto>()
                .IncludeAllDerived();
        }
    }
}