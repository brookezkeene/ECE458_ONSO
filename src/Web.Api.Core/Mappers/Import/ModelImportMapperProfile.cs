using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Core.Services;

namespace Web.Api.Core.Mappers.Import
{
    public class ModelImportMapperProfile : Profile
    {
        public ModelImportMapperProfile()
        {
            CreateMap<ImportModelDto, ModelDto>()
                .ForMember(o => o.NetworkPorts, opts => opts.MapFrom<ImportModelNetworkPortResolver>())
                .ForMember(o => o.Assets, opts => opts.Ignore())
                .ForMember(o => o.Id, opts => opts.Ignore())
                .AfterMap<HydrateModelAction>();
        }
    }
}