﻿using System;
using AutoMapper;
using Web.Api.Common.Extensions;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Mappers.Import.Assets
{
    public class AssetImportMapperProfile : Profile
    {
        public AssetImportMapperProfile()
        {
            CreateMap<ImportAssetDto, AssetDto>()
                .ForMember(o => o.Id, opts => opts.MapFrom<ImportAssetLookupResolver>())
                .ForMember(o => o.ModelId, opts => opts.MapFrom<ImportAssetModelLookupResolver>())
                .ForMember(o => o.RackId, opts => opts.MapFrom<ImportAssetRackLookupResolver>())
                .ForMember(o => o.OwnerId, opts => opts.MapFrom<ImportAssetUserLookupResolver>())
                .ForMember(x => x.ChassisId, opts => opts.MapFrom<ImportBladeChassisLookupResolver>())
                // we only ignore complex types!
                .ForMember(o => o.Model, opts => opts.Ignore())
                .ForMember(o => o.Rack, opts => opts.Ignore())
                .ForMember(o => o.Owner, opts => opts.Ignore())
                .ForMember(o => o.NetworkPorts, opts => opts.Ignore())
                .ForMember(o => o.PowerPorts, opts => opts.Ignore())
                .ForMember(o => o.Blades, opts => opts.Ignore())
                .ForMember(o => o.Chassis, opts => opts.Ignore())
                .ForMember(o => o.LastUpdatedDate, opts => opts.MapFrom(src => DateTime.Now))
                .AfterMap<HydrateAssetPowerPorts>();
        }
    }
}