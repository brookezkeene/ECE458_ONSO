﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Dtos
{
    public class CreateModelApiDto
    {
        public string Vendor { get; set; }
        public string ModelNumber { get; set; }
        public int Height { get; set; }
        public string DisplayColor { get; set; }
        public string Cpu { get; set; }
        public string Storage { get; set; }
        public string Comment { get; set; }
        public int? Memory { get; set; }
        public int? EthernetPorts { get; set; }
        public int? PowerPorts { get; set; }
        public List<CreateModelNetworkPortDto> NetworkPorts { get; set; }
    }
    public class CreateModelNetworkPortDto
    {
        public string Name { get; set; }
    }
}
