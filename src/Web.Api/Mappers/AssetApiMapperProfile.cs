using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Dtos;

namespace Web.Api.Mappers
{
    public class AssetApiMapperProfile : Profile
    {
        public AssetApiMapperProfile()
        {
            CreateMap<AssetDto, GetAssetApiDto>(MemberList.Destination);
        }
    }
}
