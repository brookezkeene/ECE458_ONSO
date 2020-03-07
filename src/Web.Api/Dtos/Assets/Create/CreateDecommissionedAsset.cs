﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Dtos.Assets.Create
{
    public class CreateDecommissionedAsset
    {
        public Guid Id { get; set; }
        public string Hostname { get; set; }
        public string Comment { get; set; }
        public int RackPosition { get; set; }
        public string OwnerName { get; set; }
        public string DecommissionedOwnerName { get; set; }
        public string TimeStamp { get; set; }
        public string Datacenter { get; set; }
        public int? AssetNumber { get; set; }
        public string RackNumber { get; set; }
        public CreateDecommissionedModel Model { get; set; }
        public List<CreateDecommissionedNetworkPort> NetworkPorts { get; set; }
    }
    public class CreateDecommissionedModel
    {
        public string Vendor { get; set; }
        public string Number { get; set; }
    }
    public class CreateDecommissionedNetworkPort
    {
        public string MacAddress { get; set; }
        public string HostName { get; set; }
        //the number and name i'll get from the model network port
        public int Number { get; set; }
        public string Name { get; set; }
        public CreateDecommissionedNetworkPort ConnectedPort { get; set; }
    }

}
