using System;

namespace Web.Api.Dtos.Assets.Read
{
    public class GetAssetNetworkPortApiDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public string MacAddress { get; set; }
        public Guid? ConnectedPortId { get; set; }
        public GetAssetNetworkPortShallowApiDto ConnectedPort { get; set; }
    }
}