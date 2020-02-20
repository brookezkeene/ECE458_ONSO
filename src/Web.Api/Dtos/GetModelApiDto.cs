using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Dtos
{
    public class GetModelApiDto : GetModelsApiDto
    {
        public List<GetModelAssetApiDto> Assets;
        public string Comment { get; set; }
        public List<string> NetworkPortNames { get; set; }
    }

    public class GetModelAssetApiDto
    {
        public Guid Id { get; set; }
        public string Hostname { get; set; }
    }

    public class GetModelsApiDto
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
    }
}
