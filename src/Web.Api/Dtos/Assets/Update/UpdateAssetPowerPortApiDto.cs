using System;

namespace Web.Api.Dtos.Assets.Update
{
    public class UpdateAssetPowerPortApiDto
    {
        public Guid Id { get; set; }
        public Guid? PduPortId { get; set; }
    }
}