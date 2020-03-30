using System;

namespace Web.Api.Core.Dtos
{
    public class PduPortDto : PowerPortDto
    {
        public PduPortDto(Guid id) : base(id) { }
        public PduPortDto() { }
        public Guid PduId { get; set; }
        public PduDto Pdu { get; set; }
        public Guid? AssetPowerPortId { get; set; }
        public AssetPowerPortDto AssetPowerPort { get; set; }

        public override string ToString()
        {
            return $"{Pdu}{Number}";
        }
    }
}