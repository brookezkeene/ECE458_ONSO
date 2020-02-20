using System.Collections.Generic;
using AutoMapper;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Mappers
{
    public static class DomainMappers
    {
        static DomainMappers()
        {
            Mapper = new MapperConfiguration(cfg => cfg.AddProfile<DomainMapperProfile>())
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static ModelDto ToDto(this Model model)
        {
            return model == null ? null : Mapper.Map<ModelDto>(model);
        }

        public static PagedList<ModelDto> ToDto(this PagedList<Model> models)
        {
            return models == null ? null : Mapper.Map<PagedList<ModelDto>>(models);
        }

        public static List<ModelDto> ToDto(this List<Model> models)
        {
            return models == null ? null : Mapper.Map<List<ModelDto>>(models);
        }

        public static Model ToEntity(this ModelDto modelDto)
        {
            return modelDto == null ? null : Mapper.Map<Model>(modelDto);
        }

        public static AssetDto ToDto(this Asset asset)
        {
            return asset == null ? null : Mapper.Map<AssetDto>(asset);
        }

        public static PagedList<AssetDto> ToDto(this PagedList<Asset> assets)
        {
            return assets == null ? null : Mapper.Map<PagedList<AssetDto>>(assets);
        }

        public static List<AssetDto> ToDto(this List<Asset> assets)
        {
            return assets == null ? null : Mapper.Map<List<AssetDto>>(assets);
        }

        public static Asset ToEntity(this AssetDto assetDto)
        {
            return assetDto == null ? null : Mapper.Map<Asset>(assetDto);
        }


        public static RackDto ToDto(this Rack rack)
        {
            return rack == null ? null : Mapper.Map<RackDto>(rack);
        }

        public static PagedList<RackDto> ToDto(this PagedList<Rack> racks)
        {
            return racks == null ? null : Mapper.Map<PagedList<RackDto>>(racks);
        }

        public static List<RackDto> ToDto(this List<Rack> racks)
        {
            return racks == null ? null : Mapper.Map<List<RackDto>>(racks);
        }


        public static PagedList<UserDto> ToDto(this PagedList<User> users)
        {
            return users == null ? null : Mapper.Map<PagedList<UserDto>>(users);
        }

        public static Rack ToEntity(this RackDto rackDto)
        {
            return rackDto == null ? null : Mapper.Map<Rack>(rackDto);
        }

        public static UserDto ToDto(this User user)
        {
            return user == null ? null : Mapper.Map<UserDto>(user);
        }

        public static User ToEntity(this UserDto userDto)
        {
            return userDto == null ? null : Mapper.Map<User>(userDto);
        }

        public static User ToEntity(this RegisterUserDto registerDto)
        {
            return registerDto == null ? null : Mapper.Map<User>(registerDto);
        }

        public static DatacenterDto ToDto(this Datacenter datacenter)
        {
            return datacenter == null ? null : Mapper.Map<DatacenterDto>(datacenter);
        }

        public static PagedList<DatacenterDto> ToDto(this PagedList<Datacenter> datacenters)
        {
            return datacenters == null ? null : Mapper.Map<PagedList<DatacenterDto>>(datacenters);
        }

        public static List<DatacenterDto> ToDto(this List<Datacenter> datacenters)
        {
            return datacenters == null ? null : Mapper.Map<List<DatacenterDto>>(datacenters);
        }

        public static Datacenter ToEntity(this DatacenterDto datacenterDto)
        {
            return datacenterDto == null ? null : Mapper.Map<Datacenter>(datacenterDto);
        }
    }
}
