using System;

namespace Web.Api.Core.Dtos
{
    public class AssetDto
    {
        public ModelDto Model { get; set; }
        public RackDto Rack { get; set; }
        public UserDto Owner { get; set; }
        public Guid Id { get; set; }
        public string Hostname { get; set; }
        public int RackPosition { get; set; }
        public string Comment { get; set; }
    }
}