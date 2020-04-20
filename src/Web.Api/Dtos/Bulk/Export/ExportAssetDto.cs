using System.Collections.Generic;

namespace Web.Api.Dtos.Bulk.Export
{
    public class ExportAssetDto
    {
        public string asset_number { get; set; }
        public string hostname { get; set; }
        public string datacenter { get; set; }
        public string offline_site { get; set; }
        public string rack { get; set; }
        public int rack_position { get; set; }
        public string chassis_number { get; set; }
        public string chassis_slot { get; set; }
        public string vendor { get; set; }
        public string model_number { get; set; }
        public string owner { get; set; }
        public string comment { get; set; }
        public List<ExportAssetPowerPortDto> power_port { get; set; }
        public string custom_display_color { get; set; }
        public string custom_cpu { get; set; }
        public string custom_memory { get; set; }
        public string custom_storage { get; set; }
        
    }
    public class ExportAssetPowerPortDto
    {
        public string power_port_location { get; set; }
        public int power_port_number { get; set; }
    }
}
