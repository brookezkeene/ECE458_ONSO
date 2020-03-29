using System;
using Web.Api.Dtos.Assets.Create;

namespace Web.Api.Dtos.Assets.Read
{
    public class GetAssetNetworkPortApiDto : AssetNetworkPortApiDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public GetAssetNetworkPortShallowApiDto ConnectedPort { get; set; }
    }
}