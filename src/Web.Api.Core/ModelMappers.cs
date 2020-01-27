using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Web.Api.Common;
using Web.Api.Infrastructure;

namespace Web.Api.Core
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

        public static ModelsDto ToModel(this PagedList<Model> models)
        {
            return models == null ? null : Mapper.Map<ModelsDto>(models);
        }
    }

    public class ModelMapperProfile : Profile
    {
        public ModelMapperProfile()
        {
            CreateMap<Model, ModelDto>(MemberList.Destination)
                .ForMember(x => x.Instances, opts => opts.MapFrom(src => src.Instances.Select(x => x.Hostname)));

            CreateMap<PagedList<Model>, ModelsDto>(MemberList.Destination)
                .ForMember(x => x.Data, opt => opt.MapFrom(src => src.Data));
        }
    }
}
