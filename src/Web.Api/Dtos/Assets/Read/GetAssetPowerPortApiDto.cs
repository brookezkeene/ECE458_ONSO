using System;

namespace Web.Api.Dtos.Assets.Read
{
    public class GetAssetPowerPortApiDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public Guid? PduPortId { get; set; }
        public string PduPort { get; set; }
    }
}