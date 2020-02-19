using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Dtos
{
    public class GetRackPdusApiDto
    {
        public Guid Id { get; set; }
        public int NumPorts { get; set; }
        public List<GetPduPortApiDto> PduPorts { get; set; }
    }

    public class GetPduPortApiDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public Guid? AssetPowerPortId { get; set; }
        public Guid? AssetId { get; set; }
        public int? AssetNumber { get; set; }
    }
}
