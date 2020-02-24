using System;
using System.Collections.Generic;

namespace Web.Api.Dtos.Models.Read
{
    public class GetModelApiDto
    {
        public Guid Id { get; set; }
        public string Vendor { get; set; }
        public string ModelNumber { get; set; }
        public int Height { get; set; }
        public string DisplayColor { get; set; }
        public int? EthernetPorts { get; set; }
        public int? PowerPorts { get; set; }
        public string Cpu { get; set; }
        public int? Memory { get; set; }
        public string Storage { get; set; }
        public List<GetModelAssetApiDto> Assets { get; set; }
        public string Comment { get; set; }
        public List<GetModelNetworkPort> NetworkPorts { get; set; }
    }

}
