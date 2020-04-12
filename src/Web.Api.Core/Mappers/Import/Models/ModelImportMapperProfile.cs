using AutoMapper;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Mappers.Import.Models
{
    public class ModelImportMapperProfile : Profile
    {
        public ModelImportMapperProfile()
        {
            CreateMap<ImportModelDto, ModelDto>()
                .ForMember(o => o.NetworkPorts, opts => opts.MapFrom<ImportModelNetworkPortResolver>())
                .ForMember(o => o.Assets, opts => opts.Ignore())
                .ForMember(o => o.Id, opts => opts.Ignore())
                .ForMember(x => x.MountType, opt => opt.Ignore())
                .AfterMap<HydrateModelAction>();
        }
    }
}