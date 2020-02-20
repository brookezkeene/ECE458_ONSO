namespace Web.Api.Dtos
{
    public class CreateDatacenterApiDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasNetworkManagedPower { get; set; }
    }
}