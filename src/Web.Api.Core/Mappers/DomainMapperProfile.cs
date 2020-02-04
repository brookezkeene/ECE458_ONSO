using System.Linq;
using AutoMapper;
using Microsoft.VisualBasic;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Mappers
{
    public class DomainMapperProfile : Profile
    {
        public DomainMapperProfile()
        {
            CreateMap<Model, ModelDto>(MemberList.Destination)
                .ReverseMap();
            CreateMap<Instance, InstanceDto>(MemberList.Destination)
                .ReverseMap();
            CreateMap<Rack, RackDto>(MemberList.Destination)
                .ReverseMap();
            CreateMap<User, UserDto>(MemberList.Destination)
                .ReverseMap();
        }
    }
}