using System;

namespace Web.Api.Dtos.Racks
{
    public class GetRackPduPortApiDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public Guid? AssetPowerPortId { get; set; }
        public Guid? AssetId { get; set; }
        public int? AssetNumber { get; set; }
    }
}