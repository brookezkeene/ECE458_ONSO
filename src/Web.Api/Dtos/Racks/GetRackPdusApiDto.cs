using System;
using System.Collections.Generic;

namespace Web.Api.Dtos.Racks
{
    public class GetRackPdusApiDto
    {
        public List<GetRackPduPortApiDto> Left { get; set; }
        public List<GetRackPduPortApiDto> Right { get; set; }
    }
}
