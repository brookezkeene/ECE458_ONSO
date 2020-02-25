using System.Collections.Generic;

namespace Web.Api.Dtos.Bulk.Export
{
    public class ExportModelDto
    {
        public string vendor { get; set; }
        public string model_number { get; set; }
        public int height { get; set; }
        public string display_color { get; set; }
        public int? ethernet_ports { get; set; }
        public int? power_ports { get; set; }
        public string cpu { get; set; }
        public int? memory { get; set; }
        public string storage { get; set; }
        public string comment { get; set; }
        public List<ExportModelNetworkPortDto> network_ports { get; set; }
    }
}
