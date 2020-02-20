using System;
using System.Collections.Generic;

namespace Web.Api.Core.Dtos
{
    public class DatacenterDto
    {
        public DatacenterDto()
        {
            Racks = new List<RackDto>();
        }
        public DatacenterDto(Guid id)
        {
            Id = id;
            Racks = new List<RackDto>();
        }
        public List<RackDto> Racks { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasNetworkManagedPower { get; set; }
    }
}
