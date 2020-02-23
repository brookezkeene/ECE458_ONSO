using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Web.Api.Common;
using SkorubaCommon = Skoruba.AuditLogging.EntityFramework.Helpers.Common;
using Web.Api.Common.Mappers;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Mappers
{
    public class AuditLogMapperProfile : PaginatedProfile
    {
        public AuditLogMapperProfile()
        {
            CreateMap(typeof(SkorubaCommon.PagedList<>), typeof(PagedList<>), MemberList.Source);
            CreateMap<AuditLog, AuditLogDto>();
        }
    }
}
