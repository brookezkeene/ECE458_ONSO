using System;
using System.Collections.Generic;

namespace Web.Api.Core.Dtos
{
    public class NetworkConnectionDto
    {
        public Guid Id { get; set; }
        public List<AssetNetworkPortDto> Ports { get; set; }
    }
}