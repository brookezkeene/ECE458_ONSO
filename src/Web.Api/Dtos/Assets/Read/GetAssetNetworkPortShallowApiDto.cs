using System;
using Web.Api.Dtos.Assets.Create;

namespace Web.Api.Dtos.Assets.Read
{
    public class GetAssetNetworkPortShallowApiDto : AssetNetworkPortApiDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Hostname { get; set; }
        public Guid AssetId { get; set; }
    }
}