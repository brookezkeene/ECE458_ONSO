namespace Web.Api.Dtos.Datacenters.Create
{
    public class CreateDatacenterApiDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasNetworkManagedPower { get; set; }
        public bool? IsOffline { get; set; }
    }
}