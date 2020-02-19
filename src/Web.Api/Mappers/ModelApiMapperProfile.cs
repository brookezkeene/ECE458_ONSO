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
                .ForMember(o => o.Assets, opts => opts.MapFrom(src => src.Assets));
            CreateMap<AssetDto, GetModelAssetApiDto>();
            CreateMap<ModelDto, GetModelsApiDto>();
        }
    }
}
