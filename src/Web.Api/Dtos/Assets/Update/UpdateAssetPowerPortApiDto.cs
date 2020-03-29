using System;
using Web.Api.Dtos.Assets.Create;

namespace Web.Api.Dtos.Assets.Update
{
    public class UpdateAssetPowerPortApiDto : AssetPowerPortApiDto
    {
        public Guid Id { get; set; }
    }
}