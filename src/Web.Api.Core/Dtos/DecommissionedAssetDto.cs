using System;
using System.Collections.Generic;
using System.Text;
using Web.Api.Core.Helpers;

namespace Web.Api.Core.Dtos
{
    public class DecommissionedAssetDto : IRackAddressed
    {
        public Guid Id { get; set; }
        public string Hostname { get; set; }
        public string Datacenter { get; set; }
        public string ModelName { get; set; }
        public string ModelNumber { get; set; }
        public string Decommissioner { get; set; }
        public DateTime DateDecommissioned { get; set; }
        public int RackPosition { get; set; }
        public string OwnerName { get; set; }
        public string RackAddress { get; set; }
        public string Data { get; set; }
    }
}
