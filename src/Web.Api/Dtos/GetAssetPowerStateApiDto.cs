using System;
using System.Collections.Generic;

namespace Web.Api.Dtos
{
    public class GetAssetPowerStateApiDto
    {
        public Guid Id { get; set; } // asset id
        public List<GetAssetPowerPortStateApiDto> PowerPorts { get; set; }
    }
}