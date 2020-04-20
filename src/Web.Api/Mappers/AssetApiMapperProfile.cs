using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Web.Api.Common.Mappers;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Dtos;
using Web.Api.Dtos.Assets;
using Web.Api.Dtos.Assets.Create;
using Web.Api.Dtos.Assets.Read;
using Web.Api.Dtos.Assets.Update;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Mappers
{
    public class AssetApiMapperProfile : PaginatedProfile
    {
        public AssetApiMapperProfile()
        {
            // Core
            CreateMap<AssetDto, AssetApiDto>()
                .ForMember(x => x.ChangePlanId, opt => opt.Ignore())
                .IncludeAllDerived()
                .ReverseMap()
                .IncludeAllDerived();

            CreateMap<AssetNetworkPortDto, AssetNetworkPortApiDto>()
                .IncludeAllDerived()
                .ReverseMap()
                .IncludeAllDerived();

            CreateMap<AssetPowerPortDto, AssetPowerPortApiDto>()
                .IncludeAllDerived()
                .ReverseMap()
                .IncludeAllDerived();

            // Create
            CreateMap<CreateAssetPowerPortApiDto, AssetPowerPortDto>(MemberList.Source);
            CreateMap<CreateAssetNetworkPortApiDto, AssetNetworkPortDto>(MemberList.Source);
            CreateMap<CreateAssetApiDto, AssetDto>(MemberList.Source)
                .ForSourceMember(x => x.ChangePlanId, opt => opt.DoNotValidate());

            // Update
            CreateMap<UpdateAssetPowerPortApiDto, AssetPowerPortDto>(MemberList.Source);
            CreateMap<UpdateAssetNetworkPortApiDto, AssetNetworkPortDto>(MemberList.Source);
            CreateMap<UpdateAssetApiDto, AssetDto>(MemberList.Source)
                .ForSourceMember(x => x.ChangePlanId, opt => opt.DoNotValidate());;

            // Read
            CreateMap<AssetDto, GetAssetsApiDto>()
                .ForMember(o => o.MountType, opts => opts.MapFrom(src => src.Model.MountType))
                .ForMember(o => o.Rack, opts => opts.MapFrom(src => src.Rack.RackAddress))
                .ForMember(o => o.Owner, opts => opts.MapFrom(src => src.Owner.Username))
                .ForMember(o => o.Datacenter, opts => opts.MapFrom(src => src.Rack.Datacenter.Name))
                .IncludeMembers(o => o.Rack, o => o.Model)
                .IncludeAllDerived();
            CreateMap<AssetDto, GetAssetApiDto>();

            CreateMap<AssetDto, GetAssetNetworkPortShallowApiDto>(MemberList.None);
            CreateMap<AssetPowerPortDto, GetAssetPowerPortApiDto>()
                .IncludeAllDerived();

            CreateMap<AssetNetworkPortDto, GetAssetNetworkPortApiDto>()
                .ForMember(o => o.Number, opts => opts.MapFrom(src => src.ModelNetworkPort.Number))
                .ForMember(o => o.Name, opts => opts.MapFrom(src => src.ModelNetworkPort.Name))
                .IncludeMembers(o => o.ModelNetworkPort);
            CreateMap<AssetNetworkPortDto, GetAssetNetworkPortShallowApiDto>()
                .IncludeMembers(o => o.ModelNetworkPort, o => o.Asset);
            CreateMap<ModelNetworkPortDto, GetAssetNetworkPortApiDto>(MemberList.None);
            CreateMap<ModelNetworkPortDto, GetAssetNetworkPortShallowApiDto>(MemberList.None);

            CreateMap<RackDto, GetAssetsApiDto>(MemberList.None)
                .IncludeMembers(o => o.Datacenter);
            CreateMap<DatacenterDto, GetAssetsApiDto>(MemberList.None);
            CreateMap<ModelDto, GetAssetsApiDto>(MemberList.None);

            CreateMap<AssetPowerStateDto, GetAssetPowerStateApiDto>();
            CreateMap<AssetPowerPortStateDto, GetAssetPowerPortStateApiDto>();

            CreateMap<AssetDto, CreateDecommissionedAsset>()
                .ForMember(o => o.NetworkPortGraph, opts => opts.Ignore())
                .ForMember(o => o.Decommissioner, opts => opts.Ignore())
                .ForMember(o => o.DateDecommissioned, opts => opts.Ignore());

            CreateMap<UpdateAssetApiDto, CreateAssetApiDto>();
            CreateMap<UpdateAssetNetworkPortApiDto, CreateAssetNetworkPortApiDto>();
            CreateMap<UpdateAssetPowerPortApiDto, CreateAssetPowerPortApiDto>();
            CreateMap<AssetDto, DecommissionedAssetDto>()
                .ForMember(o => o.Datacenter, opts => opts.MapFrom(src => src.Rack.Datacenter.Name))
                .ForMember(o => o.RackAddress, opts => opts.MapFrom(src => src.Rack.RackAddress))
                .ForMember(o => o.ModelName, opts => opts.MapFrom(src => src.Model.Vendor))
                .ForMember(o => o.ModelNumber, opts => opts.MapFrom(src => src.Model.ModelNumber))
                .ForMember(o => o.Hostname, opts => opts.MapFrom(src => src.Hostname))
                .ForMember(o => o.OwnerName, opts => opts.MapFrom(src => src.Owner.Username))
                .ForMember(o => o.Decommissioner, opts => opts.Ignore())
                .ForMember(o => o.Data, opts => opts.Ignore())
                .ForMember(o => o.DateDecommissioned, opts => opts.Ignore())
                .ReverseMap();
            CreateMap<CreateDecommissionedAsset, DecommissionedAssetDto>()
                .ForMember(o => o.Datacenter, opts => opts.MapFrom(src => src.Datacenter))
                .ForMember(o => o.RackAddress, opts => opts.MapFrom(src => src.Rack))
                .ForMember(o => o.ModelName, opts => opts.MapFrom(src => src.Vendor))
                .ForMember(o => o.OwnerName, opts => opts.MapFrom(src => src.Owner))
                .ForMember(o => o.Data, opts => opts.Ignore())
                .ReverseMap();
        }
    }
}
