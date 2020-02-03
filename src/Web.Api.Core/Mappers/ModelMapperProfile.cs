using System.Linq;
using AutoMapper;
using Microsoft.VisualBasic;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Mappers
{
    public class ModelMapperProfile : Profile
    {
        public ModelMapperProfile()
        {
            // entity -> dto
            CreateMap<Model, ModelDto>(MemberList.Destination)
                .ReverseMap();


        }
    }
}