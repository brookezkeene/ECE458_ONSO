using System;

namespace Web.Api.Dtos.Assets.Read
{
    public class GetAssetNetworkPortShallowApiDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Hostname { get; set; }
        public Guid AssetId { get; set; }
        public string MacAddress { get; set; }
    }
}