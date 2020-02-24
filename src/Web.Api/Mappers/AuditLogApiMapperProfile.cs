using Web.Api.Common.Mappers;
using Web.Api.Core.Dtos;
using Web.Api.Dtos.AuditEvents;

namespace Web.Api.Mappers
{
    public class AuditLogApiMapperProfile : PaginatedProfile
    {
        public AuditLogApiMapperProfile()
        {
            CreateMap<AuditLogDto, GetAuditLogApiDto>();
        }
    }
}