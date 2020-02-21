using System;
using System.Collections.Generic;

namespace Web.Api.Dtos.Racks
{
    public class GetRackPdusApiDto
    {
        public Guid Id { get; set; }
        public int NumPorts { get; set; }
        public List<GetRackPduPortApiDto> PduPorts { get; set; }
    }
}
