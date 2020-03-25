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

        public static void AssertConfigurationIsValid<TProfile>() where TProfile : Profile, new()
        {
            Mapper.ConfigurationProvider.AssertConfigurationIsValid<TProfile>();
        }

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

        public static ModelNetworkPortDto ToDto(this ModelNetworkPort modelNetworkPort)
        {
            return Mapper.Map<ModelNetworkPortDto>(modelNetworkPort);
        }

        public static List<ModelNetworkPortDto> ToDto(this List<ModelNetworkPort> list)
        {
            return Mapper.Map<List<ModelNetworkPortDto>>(list);
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
        public static List<AssetNetworkPortDto> ToDto(this List<AssetNetworkPort> ports)
        {
            return ports == null ? null : Mapper.Map<List<AssetNetworkPortDto>>(ports);
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
        public static DecommissionedAsset ToEntity(this DecommissionedAssetDto decommissionedAssetDto)
        {
            return decommissionedAssetDto == null ? null : Mapper.Map<DecommissionedAsset>(decommissionedAssetDto);
        }
        public static DecommissionedAssetDto ToDto(this DecommissionedAsset decommissionedAsset)
        {
            return decommissionedAsset == null ? null : Mapper.Map<DecommissionedAssetDto>(decommissionedAsset);
        }
        public static PagedList<DecommissionedAssetDto> ToDto(this PagedList<DecommissionedAsset> assets)
        {
            return assets == null ? null : Mapper.Map<PagedList<DecommissionedAssetDto>>(assets);
        }
        public static ChangePlan ToEntity(this ChangePlanDto changePlanDto)
        {
            return changePlanDto == null ? null : Mapper.Map<ChangePlan>(changePlanDto);
        }
        
        public static ChangePlanItem ToEntity(this ChangePlanItemDto changePlanItemDto)
        {
            return changePlanItemDto == null ? null : Mapper.Map<ChangePlanItem>(changePlanItemDto);
        }
        
        public static ChangePlanDto ToDto(this ChangePlan changePlan)
        {
            return changePlan == null ? null : Mapper.Map<ChangePlanDto>(changePlan);
        }
        public static PagedList<ChangePlanDto> ToDto(this PagedList<ChangePlan> changePlan)
        {
            return changePlan == null ? null : Mapper.Map<PagedList<ChangePlanDto>>(changePlan);
        }
        public static ChangePlanItemDto ToDto(this ChangePlanItem changePlanItem)
        {
            return changePlanItem == null ? null : Mapper.Map<ChangePlanItemDto>(changePlanItem);
        }
        public static List<ChangePlanItemDto> ToDto(this List<ChangePlanItem> changePlanItem)
        {
            return changePlanItem == null ? null : Mapper.Map<List<ChangePlanItemDto>>(changePlanItem);
        }
    }
}
