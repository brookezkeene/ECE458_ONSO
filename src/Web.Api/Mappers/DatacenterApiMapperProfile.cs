using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Web.Api.Core.Dtos;
using Web.Api.Dtos;

namespace Web.Api.Mappers
{
    public class DatacenterApiMapperProfile : Profile
    {
        public DatacenterApiMapperProfile()
        {
            CreateMap<DatacenterDto, GetDatacenterApiDto>()
                .ReverseMap();

            CreateMap<DatacenterDto, CreateDatacenterApiDto>()
                .ReverseMap();

            CreateMap<DatacenterDto, UpdateDatacenterApiDto>()
                .ReverseMap();
        }
    }
}
