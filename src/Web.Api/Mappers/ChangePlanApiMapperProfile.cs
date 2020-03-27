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
using Web.Api.Dtos.ChangePlans;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Mappers
{
    public class ChangePlanApiMapperProfile : PaginatedProfile
    {
        public ChangePlanApiMapperProfile()
        {

            // Create
            CreateMap<CreateChangePlanApiDto, ChangePlanDto>(MemberList.Source);
            CreateMap<CreateChangePlanItemApiDto, ChangePlanItemDto>(MemberList.Source);

            // Update
            CreateMap<UpdateChangePlanItemApiDto, ChangePlanItemDto>(MemberList.Source);

            // Read
            CreateMap<ChangePlanDto, GetChangePlanApiDto>()
                .IncludeAllDerived();
            CreateMap<ChangePlanItemDto, GetChangePlanItemApiDto>();

        }
    }
}
