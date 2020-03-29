using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Web.Api.Infrastructure.Entities.Extensions
{
    public static class AssetNetworkPortExtensions
    {
        public static AssetNetworkPort ConnectedPort(this AssetNetworkPort port)
        {
            return port.NetworkConnection?.Ports.Single(connectedPort => connectedPort.Id != port.Id);
        }

        public static Guid? ConnectedPortId(this AssetNetworkPort port)
        {
            return port.ConnectedPort()
                ?.Id;
        }
    }
}
