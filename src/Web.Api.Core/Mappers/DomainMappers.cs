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

        public static RackDto ToDto(this Rack rack)
        {
            return rack == null ? null : Mapper.Map<RackDto>(rack);
        }

        public static ModelDto ToDto(this Model model)
        {
            return model == null ? null : Mapper.Map<ModelDto>(model);
        }

        public static InstanceDto ToDto(this Instance instance)
        {
            return instance == null ? null : Mapper.Map<InstanceDto>(instance);
        }
        public static UserDto ToDto(this User user)
        {
            return user == null ? null : Mapper.Map<UserDto>(user);
        }

        public static PagedList<RackDto> ToDto(this PagedList<Rack> racks)
        {
            return racks == null ? null : Mapper.Map<PagedList<RackDto>>(racks);
        }

        public static PagedList<ModelDto> ToDto(this PagedList<Model> models)
        {
            return models == null ? null : Mapper.Map<PagedList<ModelDto>>(models);
        }

        public static PagedList<InstanceDto> ToDto(this PagedList<Instance> instances)
        {
            return instances == null ? null : Mapper.Map<PagedList<InstanceDto>>(instances);
        }
        public static PagedList<UserDto> ToDto(this PagedList<User> users)
        {
            return users == null ? null : Mapper.Map<PagedList<UserDto>>(users);
        }

        public static Rack ToEntity(this RackDto rackDto)
        {
            return rackDto == null ? null : Mapper.Map<Rack>(rackDto);
        }

        public static Model ToEntity(this ModelDto modelDto)
        {
            return modelDto == null ? null : Mapper.Map<Model>(modelDto);
        }

        public static Instance ToEntity(this InstanceDto instanceDto)
        {
            return instanceDto == null ? null : Mapper.Map<Instance>(instanceDto);
        }

        public static User ToEntity(this UserDto userDto)
        {
            return userDto == null ? null : Mapper.Map<User>(userDto);
        }
    }
}
