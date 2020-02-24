using System;

namespace Web.Api.Dtos.Assets.Create
{
    public class CreateAssetPowerPortApiDto
    {
        public int Number { get; set; }
        public Guid? PduPortId { get; set; }
    }
}