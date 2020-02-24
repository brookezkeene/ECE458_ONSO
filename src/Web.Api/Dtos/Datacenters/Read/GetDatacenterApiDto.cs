using System;

namespace Web.Api.Dtos.Datacenters.Read
{
    public class GetDatacenterApiDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasNetworkManagedPower { get; set; }
    }
}
