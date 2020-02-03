using AutoMapper;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Mappers
{
    public static class ModelMappers
    {
        static ModelMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<ModelMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static ModelDto ToModel(this Model model)
        {
            return model == null ? null : Mapper.Map<ModelDto>(model);
        }

        public static PagedList<ModelDto> ToModel(this PagedList<Model> models)
        {
            return models == null ? null : Mapper.Map<PagedList<ModelDto>>(models);
        }

        public static Model ToEntity(this ModelDto modelDto)
        {
            return modelDto == null ? null : Mapper.Map<Model>(modelDto);
        }
    }
}
