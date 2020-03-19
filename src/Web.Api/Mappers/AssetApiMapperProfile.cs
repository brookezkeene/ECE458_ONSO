using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Web.Api.Common.Mappers;
using Web.Api.Core.Dtos;
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
                .IncludeAllDerived()
                .ReverseMap();

            // Create
            CreateMap<CreateAssetPowerPortApiDto, AssetPowerPortDto>(MemberList.Source);
            CreateMap<CreateAssetNetworkPortApiDto, AssetNetworkPortDto>(MemberList.Source);
            CreateMap<CreateAssetApiDto, AssetDto>(MemberList.Source);

            // Update
            CreateMap<UpdateAssetPowerPortApiDto, AssetPowerPortDto>(MemberList.Source);
            CreateMap<UpdateAssetNetworkPortApiDto, AssetNetworkPortDto>(MemberList.Source);
            CreateMap<UpdateAssetApiDto, AssetDto>(MemberList.Source);

            // Read
            CreateMap<AssetDto, GetAssetsApiDto>()
                .ForMember(o => o.Rack, opts => opts.MapFrom(src => src.Rack.Address))
                .ForMember(o => o.Owner, opts => opts.MapFrom(src => src.Owner.Username))
                .ForMember(o => o.Datacenter, opts => opts.MapFrom(src => src.Rack.Datacenter.Name))
                .IncludeMembers(o => o.Rack, o => o.Model)
                .IncludeAllDerived();
            CreateMap<AssetDto, GetAssetApiDto>();

            CreateMap<AssetDto, GetAssetNetworkPortShallowApiDto>(MemberList.None);
            CreateMap<AssetPowerPortDto, GetAssetPowerPortApiDto>()
                .IncludeAllDerived();

            CreateMap<AssetNetworkPortDto, GetAssetNetworkPortApiDto>()
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
                .ForMember(o => o.OwnerName, opts => opts.MapFrom(src => src.Owner.Username))
                .ForMember(o => o.NetworkPortGraph, opts => opts.Ignore());
            CreateMap<RackDto, CreateDecommissionedRack>()
                .ForMember(o => o.RackLetter, opts => opts.MapFrom(src => src.RowLetter));
            CreateMap<ModelDto, CreateDecommissionedModel>()
                .ForMember(o => o.Number, opts => opts.MapFrom(src => src.ModelNumber));
            CreateMap<AssetNetworkPortDto, CreateDecommissionedNetworkPort>()
                .ForMember(o => o.HostName, opts => opts.MapFrom(src => src.Asset.Hostname))
                .ForMember(o => o.Number, opts => opts.MapFrom(src => src.ModelNetworkPort.Number))
                .ForMember(o => o.Name, opts => opts.MapFrom(src => src.ModelNetworkPort.Name))
                .ReverseMap();
            CreateMap<AssetPowerPortDto, CreateDecommissionedPowerPort>();
            CreateMap<AssetDto, DecommissionedAssetDto>()
                .ForMember(o => o.Datacenter, opts => opts.MapFrom(src => src.Rack.Datacenter.Name))
                .ForMember(o => o.RackAddress, opts => opts.MapFrom(src => src.Rack.Address))
                .ForMember(o => o.ModelName, opts => opts.MapFrom(src => src.Model.Vendor))
                .ForMember(o => o.ModelNumber, opts => opts.MapFrom(src => src.Model.ModelNumber))
                .ForMember(o => o.Hostname, opts => opts.MapFrom(src => src.Hostname))
                .ForMember(o => o.Decommissioner, opts => opts.Ignore())
                .ForMember(o => o.Data, opts => opts.Ignore())
                .ForMember(o => o.Date, opts => opts.Ignore())
                .ReverseMap();
        }
    }
}
