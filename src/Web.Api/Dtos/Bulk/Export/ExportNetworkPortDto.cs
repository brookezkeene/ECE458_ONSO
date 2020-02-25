using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Dtos.Bulk.Export
{
    public class ExportNetworkPortAssetDto {
        public List<ExportNetworkPortDto> network_ports {get; set;}
    }
    public class ExportNetworkPortDto
    {
        public string src_hostname { get; set; }
        public string src_port { get; set; }
        public string src_mac { get; set; }
        public string dest_hostname { get; set; }
        public string dest_port { get; set; }
    }
}
