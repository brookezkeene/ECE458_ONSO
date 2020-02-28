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

            CreateMap<AssetNetworkPortDto, GetAssetNetworkPortFromDatacenterDto>()
                .ForMember(o => o.Name, opts => opts.MapFrom(src => src.ModelNetworkPort.Name))
                .ForMember(o => o.AssetHostname, opts => opts.MapFrom(src => src.Asset.Hostname))
                .ForMember(o => o.RowLetter, opts => opts.MapFrom(src => src.Asset.Rack.RowLetter))
                .ForMember(o => o.RackNumber, opts => opts.MapFrom(src => src.Asset.Rack.RackNumber))
                .ForMember(o => o.Id, opts => opts.MapFrom(src => src.Id));
        }
    }
}
