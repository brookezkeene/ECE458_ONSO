using System;

namespace Web.Api.Dtos
{
    public class UpdateAssetApiDto
    {
        public Guid Id { get; set; }
        public string Rack { get; set; }
        public string Hostname { get; set; }
        public string Comment { get; set; }
        public int RackPosition { get; set; }
        public string Owner { get; set; }
    }
}