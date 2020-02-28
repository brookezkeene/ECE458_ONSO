using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Dtos.Power;

namespace Web.Api.Core.Dtos
{

    public class AssetPowerStateDto
    {
        public Guid Id { get; set; }
        public List<AssetPowerPortStateDto> PowerPorts { get; set; }
    }
    public class AssetPowerPortStateDto
    {
        public string Port { get; set; }
        public PowerState Status { get; set; }
    }
}
