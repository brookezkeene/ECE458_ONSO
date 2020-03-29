using System;
using Web.Api.Dtos.Assets.Create;

namespace Web.Api.Dtos.Assets.Read
{
    public class GetAssetPowerPortApiDto : AssetPowerPortApiDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public string PduPort { get; set; }
    }
}