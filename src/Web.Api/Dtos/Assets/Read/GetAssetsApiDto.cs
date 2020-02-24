using System;

namespace Web.Api.Dtos.Assets.Read
{
    public class GetAssetsApiDto : AssetApiDto
    {
        public string Rack { get; set; }
        public int Height { get; set; }
        public string Vendor { get; set; }
        public string ModelNumber { get; set; }
        public string DisplayColor { get; set; }
        public string Owner { get; set; }
        public Guid DatacenterId { get; set; }
        public string Datacenter { get; set; }
        public bool HasNetworkManagedPower { get; set; }
        public Guid Id { get; set; }
    }
}
