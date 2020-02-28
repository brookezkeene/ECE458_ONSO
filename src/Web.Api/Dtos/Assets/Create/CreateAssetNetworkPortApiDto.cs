using System;

namespace Web.Api.Dtos.Assets.Create
{
    public class CreateAssetNetworkPortApiDto
    {
        public Guid ModelNetworkPortId { get; set; }
        public string MacAddress { get; set; }
        public Guid? ConnectedPortId { get; set; }
    }
}