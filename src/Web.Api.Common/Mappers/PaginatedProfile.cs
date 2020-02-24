using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;

namespace Web.Api.Common.Mappers
{
    public class PaginatedProfile : Profile
    {
        public PaginatedProfile()
        {
            CreateMap(typeof(PagedList<>), typeof(PagedList<>));
        }
    }
}
