using System;

namespace Web.Api.Core.Dtos
{
    public class AssetNetworkPortDto
    {
        public Guid Id { get; set; }
        public string MacAddress { get; set; }
        public Guid AssetId { get; set; }
        public AssetDto Asset { get; set; }
        public Guid ModelNetworkPortId { get; set; }
        public ModelNetworkPortDto ModelNetworkPort { get; set; }
        public Guid? ConnectedPortId { get; set; }
        public AssetNetworkPortDto ConnectedPort { get; set; }
    }
}