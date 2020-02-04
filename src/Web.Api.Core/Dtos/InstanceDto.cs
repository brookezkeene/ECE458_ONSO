namespace Web.Api.Core.Dtos
{
    public class InstanceDto : FlatInstanceDto
    {
        public FlatModelDto Model { get; set; }
        public string Rack { get; set; }
        public FlatUserDto Owner { get; set; }
    }
}