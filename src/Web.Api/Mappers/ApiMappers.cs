using AutoMapper;

namespace Web.Api.Mappers
{
    public static class ApiMappers
    {
        static ApiMappers()
        {
            Mapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AssetApiMapperProfile>();
                    cfg.AddProfile<ModelApiMapperProfile>();
                    cfg.AddProfile<ExportMapperProfile>();
                    cfg.AddProfile<DatacenterApiMapperProfile>();
                    cfg.AddProfile<AuditLogApiMapperProfile>();
                })
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static T MapTo<T>(this object source)
        {
            return Mapper.Map<T>(source);
        }

        public static void AssertConfigurationIsValid<TProfile>() where TProfile : Profile, new()
        {
            Mapper.ConfigurationProvider.AssertConfigurationIsValid<TProfile>();
        }
    }
}
