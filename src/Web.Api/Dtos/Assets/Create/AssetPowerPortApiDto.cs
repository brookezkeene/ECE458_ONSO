using System;

namespace Web.Api.Dtos.Assets.Create
{
    public abstract class AssetPowerPortApiDto
    {
        public Guid? PduPortId { get; set; }
    }
}