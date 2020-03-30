using System;

namespace Web.Api.Core.Dtos
{
    public class AssetPowerPortDto : PowerPortDto
    {
        public AssetPowerPortDto(Guid id) : base(id) { }
        public AssetPowerPortDto() { }
        public Guid AssetId { get; set; }
        public AssetDto Asset { get; set; }
        public Guid? PduPortId { get; set; }
        public PduPortDto PduPort { get; set; }
    }
}