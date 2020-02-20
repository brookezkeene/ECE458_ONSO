using System;
using System.Collections.Generic;

namespace Web.Api.Dtos
{
    public class UpdateAssetApiDto
    {
        public Guid Id { get; set; }
        public Guid RackId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid ModelId { get; set; }
        public string Hostname { get; set; }
        public string Comment { get; set; }
        public int RackPosition { get; set; }
        public List<UpdateAssetPowerPortApiDto> PowerPorts { get; set; }
        public List<UpdateAssetNetworkPortApiDto> NetworkPorts { get; set; }
    }

    public class UpdateAssetNetworkPortApiDto
    {
        public Guid Id { get; set; }
        public string MacAddress { get; set; }
        public Guid? ConnectedPortId { get; set; }
    }

    public class UpdateAssetPowerPortApiDto
    {
        public Guid Id { get; set; }
        public Guid? PduPortId { get; set; }
    }
}