using System;

namespace Web.Api.Dtos.Assets.Update
{
    public class UpdateAssetNetworkPortApiDto
    {
        public Guid Id { get; set; }
        public string MacAddress { get; set; }
        public Guid? ConnectedPortId { get; set; }
    }
}