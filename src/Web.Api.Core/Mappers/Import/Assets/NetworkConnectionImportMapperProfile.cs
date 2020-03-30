using AutoMapper;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Mappers.Import.Assets
{
    public class NetworkConnectionImportMapperProfile : Profile
    {
        public NetworkConnectionImportMapperProfile()
        {
            CreateMap<ImportNetworkConnectionDto, NetworkConnectionDto>()
                .ConvertUsing<ImportNetworkConnectionConverter>();
        }
    }
}