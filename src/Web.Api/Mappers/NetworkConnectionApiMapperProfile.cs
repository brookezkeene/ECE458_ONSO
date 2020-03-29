using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Web.Api.Controllers;
using Web.Api.Core.Dtos;
using Web.Api.Dtos;

namespace Web.Api.Mappers
{
    public class NetworkConnectionApiMapperProfile : Profile
    {
        public NetworkConnectionApiMapperProfile()
        {
            CreateMap<CreateNetworkConnectionApiDto, NetworkConnectionDto>(MemberList.Source);
            CreateMap<CreateNetworkConnectionPortApiDto, AssetNetworkPortDto>(MemberList.Source);
        }
    }
}
