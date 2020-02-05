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

        #region Models
        public static ModelDto ToDto(this Model model)
        {
            return model == null ? null : Mapper.Map<ModelDto>(model);
        }
        public static FlatModelDto ToFlatDto(this Model model)
        {
            return model == null ? null : Mapper.Map<FlatModelDto>(model);
        }
        public static PagedList<ModelDto> ToDto(this PagedList<Model> models)
        {
            return models == null ? null : Mapper.Map<PagedList<ModelDto>>(models);
        }
        public static PagedList<FlatModelDto> ToFlatDto(this PagedList<Model> models)
        {
            return models == null ? null : Mapper.Map<PagedList<FlatModelDto>>(models);
        }
        public static Model ToEntity(this ModelDto modelDto)
        {
            return modelDto == null ? null : Mapper.Map<Model>(modelDto);
        }
        #endregion

        #region Instances

        public static InstanceDto ToDto(this Instance instance)
        {
            return instance == null ? null : Mapper.Map<InstanceDto>(instance);
        }

        public static FlatInstanceDto ToFlatDto(this Instance instance)
        {
            return instance == null ? null : Mapper.Map<FlatInstanceDto>(instance);
        }

        public static PagedList<InstanceDto> ToDto(this PagedList<Instance> instances)
        {
            return instances == null ? null : Mapper.Map<PagedList<InstanceDto>>(instances);
        }

        public static PagedList<FlatInstanceDto> ToFlatDto(this PagedList<Instance> instances)
        {
            return instances == null ? null : Mapper.Map<PagedList<FlatInstanceDto>>(instances);
        }

        public static Instance ToEntity(this InstanceDto instanceDto)
        {
            return instanceDto == null ? null : Mapper.Map<Instance>(instanceDto);
        }

        #endregion

        public static RackDto ToDto(this Rack rack)
        {
            return rack == null ? null : Mapper.Map<RackDto>(rack);
        }

        public static PagedList<RackDto> ToDto(this PagedList<Rack> racks)
        {
            return racks == null ? null : Mapper.Map<PagedList<RackDto>>(racks);
        }

        public static PagedList<FlatRackDto> ToFlatDto(this PagedList<Rack> racks)
        {
            return racks == null ? null : Mapper.Map<PagedList<FlatRackDto>>(racks);
        }

        public static List<RackDto> ToDto(this List<Rack> racks)
        {
            return racks == null ? null : Mapper.Map<List<RackDto>>(racks);
        }


        public static PagedList<FlatUserDto> ToDto(this PagedList<User> users)
        {
            return users == null ? null : Mapper.Map<PagedList<FlatUserDto>>(users);
        }

        public static Rack ToEntity(this RackDto rackDto)
        {
            return rackDto == null ? null : Mapper.Map<Rack>(rackDto);
        }

        public static FlatUserDto ToDto(this User user)
        {
            return user == null ? null : Mapper.Map<FlatUserDto>(user);
        }

        public static User ToEntity(this FlatUserDto flatUserDto)
        {
            return flatUserDto == null ? null : Mapper.Map<User>(flatUserDto);
        }

        public static User ToEntity(this RegisterUserDto registerDto)
        {
            return registerDto == null ? null : Mapper.Map<User>(registerDto);
        }
    }
}
