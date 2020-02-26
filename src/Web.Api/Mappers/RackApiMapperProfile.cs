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
using Web.Api.Dtos.Models.Read;
using Web.Api.Dtos.Racks;

namespace Web.Api.Mappers
{
    public class RackApiMapperProfile : PaginatedProfile
    {
        public RackApiMapperProfile()
        {
            CreateMap<RackDto, GetRacksApiDto>()
                .ReverseMap();
            CreateMap<AssetDto, GetRackAssetApiDto>()
                .ReverseMap();
            CreateMap<ModelDto, GetModelApiDto>()
                .ReverseMap();
            CreateMap<PduDto, GetRackPdusApiDto>()
                .ReverseMap();
            CreateMap<DatacenterDto, RackDatacenterApiDto>()
                .ReverseMap();
        }
    }
}
