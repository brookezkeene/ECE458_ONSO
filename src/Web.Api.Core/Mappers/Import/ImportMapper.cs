using AutoMapper;

namespace Web.Api.Core.Mappers.Import
{
    public static class ImportMapper
    {
        static ImportMapper()
        {
            Mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ModelImportMapperProfile>();
            }).CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static void AssertConfigurationIsValid<TProfile>() where TProfile : Profile, new()
        {
            Mapper.ConfigurationProvider.AssertConfigurationIsValid<TProfile>();
        }
    }
}
