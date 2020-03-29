using System;

namespace Web.Api.Dtos.Assets.Create
{
    public abstract class AssetNetworkPortApiDto
    {
        public Guid ModelNetworkPortId { get; set; }
        public string MacAddress { get; set; }
        public Guid? ConnectedPortId { get; set; }
    }
}