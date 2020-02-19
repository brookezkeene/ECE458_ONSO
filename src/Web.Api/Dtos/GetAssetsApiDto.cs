using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Dtos
{
    public class GetAssetsApiDto
    {
        public Guid Id { get; set; }
        public string Hostname { get; set; }
        public Guid RackId { get; set; }
        public string Rack { get; set; }
        public int RackPosition { get; set; }
        public int Height { get; set; }
        public string Comment { get; set; }
        public string Vendor { get; set; }
        public string ModelNumber { get; set; }
        public string DisplayColor { get; set; }
        public Guid ModelId { get; set; }
        public Guid? OwnerId { get; set; }
        public string Owner { get; set; }
        public int AssetNumber { get; set; }
        public Guid DatacenterId { get; set; }
        public string Datacenter { get; set; }
        public bool HasNetworkManagedPower { get; set; }

    }

    public class GetAssetApiDto : GetAssetsApiDto
    {
        public IEnumerable<int> SlotsOccupied { get; set; }
        public List<GetAssetPowerPortApiDto> PowerPorts { get; set; }
        public List<GetAssetNetworkPortApiDto> NetworkPorts { get; set; } 
    }

    public class GetAssetNetworkPortApiDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MacAddress { get; set; }
        public Guid? ConnectedPortId { get; set; }
        public GetAssetNetworkPortShallowApiDto ConnectedPort { get; set; }
    }

    public class GetAssetNetworkPortShallowApiDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Hostname { get; set; }
        public Guid AssetId { get; set; }
        public string MacAddress { get; set; }
    }

    public class GetAssetPowerPortApiDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public Guid? PduPortId { get; set; }
        public string PduPort { get; set; }
    }
}
