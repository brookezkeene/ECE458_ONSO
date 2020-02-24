using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Common.Mappers;
using Web.Api.Core.Dtos;
using Web.Api.Dtos;
using Web.Api.Dtos.Datacenters;
using Web.Api.Dtos.Datacenters.Create;
using Web.Api.Dtos.Datacenters.Read;
using Web.Api.Dtos.Datacenters.Update;

namespace Web.Api.Mappers
{
    public class DatacenterApiMapperProfile : PaginatedProfile
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
