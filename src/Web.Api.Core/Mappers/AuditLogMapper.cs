using AutoMapper;
using System.Collections.Generic;
using System.Text;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.AuditLogging.EntityFramework.Helpers.Common;

namespace Web.Api.Core.Mappers
{
    public static class AuditLogMapper
    {
        static AuditLogMapper()
        {
            Mapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AuditLogMapperProfile>();
                })
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static T MapTo<T>(this PagedList<AuditLog> list)
        {
            return Mapper.Map<T>(list);
        }

        public static void AssertConfigurationIsValid<TProfile>() where TProfile : Profile, new()
        {
            Mapper.ConfigurationProvider.AssertConfigurationIsValid<TProfile>();
        }
    }
}
