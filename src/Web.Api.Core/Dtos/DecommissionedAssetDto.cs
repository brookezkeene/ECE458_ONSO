using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dtos
{
    public class DecommissionedAssetDto
    {
        public Guid Id { get; set; }
        public string Hostname { get; set; }
        public string Datacenter { get; set; }
        public string ModelName { get; set; }
        public string ModelNumber { get; set; }
        public string Decommissioner { get; set; }
        public string Date { get; set; }
        public string RackAddress { get; set; }
        public string Data { get; set; }
    }
}
