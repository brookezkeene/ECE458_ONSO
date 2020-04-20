using System;

namespace Web.Api.Dtos.Datacenters.Update
{
    public class UpdateDatacenterApiDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasNetworkManagedPower { get; set; }
        public bool? IsOffline { get; set; }
    }
}