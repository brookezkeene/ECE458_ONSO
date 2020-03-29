using System;
using Web.Api.Dtos.Assets.Create;

namespace Web.Api.Dtos.Assets.Update
{
    public class UpdateAssetNetworkPortApiDto : AssetNetworkPortApiDto
    {
        public Guid Id { get; set; }
    }
}