﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Dtos
{
    public class CreateAssetApiDto
    {
        public Guid RackId { get; set; }
        public string Hostname { get; set; }
        public string Comment { get; set; }
        public int RackPosition { get; set; }
        public Guid OwnerId { get; set; }
        public Guid ModelId { get; set; }
        public int? AssetNumber { get; set; }
        public List<CreateAssetPowerPortApiDto> PowerPorts { get; set; }
        public List<CreateAssetNetworkPortApiDto> NetworkPorts { get; set; }
    }

    public class CreateAssetNetworkPortApiDto
    {
        public string MacAddress { get; set; }
        public Guid? ConnectedPortId { get; set; }
    }

    public class CreateAssetPowerPortApiDto
    {
        public int Number { get; set; }
        public Guid? PduPortId { get; set; }
    }
}
