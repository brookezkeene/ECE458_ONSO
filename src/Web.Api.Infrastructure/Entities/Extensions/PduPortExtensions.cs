using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Api.Infrastructure.Entities.Extensions
{
    public static class PduPortExtensions
    {
        public static AssetPowerPort AssetPowerPort(this PduPort port)
        {
            return port.PowerConnection?.Ports.Single(o => o is AssetPowerPort) as AssetPowerPort;
        }

        public static Guid? AssetPowerPortId(this PduPort port)
        {
            return port.AssetPowerPort()
                ?.Id;
        }
    }
}
