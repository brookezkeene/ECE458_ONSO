using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Web.Api.Core.Dtos;
using Web.Api.Dtos;

namespace Web.Api.Mappers
{
    public class AssetApiMapperProfile : Profile
    {
        public AssetApiMapperProfile()
        {
            CreateMap<AssetDto, GetAssetsApiDto>()
                .ForMember(o => o.Vendor, opts => opts.MapFrom(src => src.Model.Vendor))
                .ForMember(o => o.ModelNumber, opts => opts.MapFrom(src => src.Model.ModelNumber))
                .ForMember(o => o.DisplayColor, opts => opts.MapFrom(src => src.Model.DisplayColor))
                .ForMember(o => o.Height, opts => opts.MapFrom(src => src.Model.Height))
                .ForMember(o => o.Rack, opts => opts.MapFrom(src => src.Rack.Address))
                .ForMember(o => o.Owner, opts => opts.MapFrom(src => src.Owner.Username));

            CreateMap<CreateAssetApiDto, AssetDto>()
                .ForMember(o => o.Id, opts => opts.Ignore())
                .ForMember(o => o.Owner, opts => opts.Ignore())
                .ForMember(o => o.Model, opts => opts.Ignore())
                .ForMember(o => o.Rack, opts => opts.Ignore());

            CreateMap<UpdateAssetApiDto, AssetDto>()
                .ForMember(o => o.Owner, opts => opts.Ignore())
                .ForMember(o => o.Model, opts => opts.Ignore())
                .ForMember(o => o.Rack, opts => opts.Ignore());
        }
    }
}
