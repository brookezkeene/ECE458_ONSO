using System.Collections.Generic;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Dtos
{
    public class RackDto : FlatRackDto
    {
        public List<FlatInstanceDto> Instances { get; set; }
    }
}