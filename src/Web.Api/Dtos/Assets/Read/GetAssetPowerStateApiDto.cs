using System;
using System.Collections.Generic;

namespace Web.Api.Dtos.Assets.Read
{
    public class GetAssetPowerStateApiDto
    {
        public Guid Id { get; set; } // asset id
        public List<GetAssetPowerPortStateApiDto> PowerPorts { get; set; }
    }
}