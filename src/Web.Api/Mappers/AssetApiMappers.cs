using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Web.Api.Dtos;

namespace Web.Api.Mappers
{
    public static class AssetApiMappers
    {
        static AssetApiMappers()
        {
            Mapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AssetApiMapperProfile>();
                    cfg.AddProfile<ExportMapperProfile>();
                })
                .CreateMapper();
        }

        internal static IMapper Mapper { get; }

        public static T MapTo<T>(this object source)
        {
            return Mapper.Map<T>(source);
        }
    }
}
