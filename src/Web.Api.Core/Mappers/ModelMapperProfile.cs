using System.Linq;
using AutoMapper;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Mappers
{
    public class ModelMapperProfile : Profile
    {
        public ModelMapperProfile()
        {
            CreateMap<Model, ModelDto>(MemberList.Destination)
                .ForMember(x => x.Instances, opts => opts.MapFrom(src => src.Instances.Select(x => x.Hostname)));

            //CreateMap<PagedList<Model>, PagedList<ModelDto>>(MemberList.Destination)
            //    .ForMember(x => x.Data, opt => opt.MapFrom(src => src.Data));
        }
    }
}