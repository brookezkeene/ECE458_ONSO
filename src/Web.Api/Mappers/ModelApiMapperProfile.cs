using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Dtos;

namespace Web.Api.Mappers
{
    public class ModelApiMapperProfile : Profile
    {
        public ModelApiMapperProfile()
        {
            CreateMap<ModelDto, GetModelApiDto>()
                .ForMember(o => o.Assets, opts => opts.MapFrom(src => src.Assets))
                .ReverseMap();
            CreateMap<AssetDto, GetModelAssetApiDto>()
                .ReverseMap();
            CreateMap<ModelDto, GetModelsApiDto>()
                .ReverseMap();
            CreateMap<ModelDto, CreateModelApiDto>()
                .ForMember(o => o.NetworkPorts, opts => opts.MapFrom(src => src.NetworkPorts))
                .ReverseMap();
            CreateMap<ModelDto, UpdateModelApiDto>()
                .ForMember(o => o.NetworkPorts, opts => opts.MapFrom(src => src.NetworkPorts))
                .ReverseMap();
            CreateMap<ModelNetworkPortDto, CreateModelNetworkPortDto>()
                .ReverseMap();
            CreateMap<ModelNetworkPortDto, UpdateModelNetworkPortDto>()
                .ReverseMap();

        }
    }
}
