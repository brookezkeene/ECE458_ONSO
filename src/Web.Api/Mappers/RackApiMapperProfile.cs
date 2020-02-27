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
using Web.Api.Dtos.Racks;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Mappers
{
    public class RackApiMapperProfile : PaginatedProfile
    {
        public RackApiMapperProfile()
        {
            CreateMap<RackDto, GetRacksApiDto>();
            CreateMap<AssetDto, GetRackAssetApiDto>()
                .IncludeMembers(o => o.Model);
            CreateMap<ModelDto, GetRackAssetApiDto>(MemberList.None);
            CreateMap<DatacenterDto, RackDatacenterApiDto>();

            CreateMap<RackDto, GetRackPdusApiDto>()
                .ForMember(o => o.Left,
                    opts => opts.MapFrom(src => src.Pdus.FirstOrDefault(pdu => pdu.Location == PduLocation.L).Ports))
                .ForMember(o => o.Right,
                    opts => opts.MapFrom(src => src.Pdus.FirstOrDefault(pdu => pdu.Location == PduLocation.R).Ports));

            CreateMap<PduPortDto, GetRackPduPortApiDto>();
        }
    }
}
