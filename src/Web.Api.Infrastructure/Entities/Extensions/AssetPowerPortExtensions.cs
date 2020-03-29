using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Api.Infrastructure.Entities.Extensions
{
    public static class AssetPowerPortExtensions
    {
        public static PduPort PduPort(this AssetPowerPort port)
        {
            return port.PowerConnection?.Ports.Single(o => o is PduPort) as PduPort;
        }

        public static Guid? PduPortId(this AssetPowerPort port)
        {
            return port.PduPort()
                ?.Id;
        }
    }
}
