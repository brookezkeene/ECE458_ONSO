using CsvHelper.Configuration.Attributes;

namespace Web.Api.Core.Dtos
{
    public class ImportModelDto
    {
        [Name("vendor")]
        public string Vendor { get; set; }

        [Name("model_number")]
        public string ModelNumber { get; set; }

        [Name("height")]
        public int Height { get; set; }

        [Name("display_color")]
        public string DisplayColor { get; set; }

        [Name("network_ports")]
        public int? EthernetPorts { get; set; }

        [Name("power_ports")]
        public int? PowerPorts { get; set; }

        [Name("cpu")]
        public string Cpu { get; set; }

        [Name("memory")]
        public int? Memory { get; set; }

        [Name("storage")]
        public string Storage { get; set; }

        [Name("comment")]
        public string Comment { get; set; }

        [Name("network_port_name_1")]
        public string NetworkPortName1 { get; set; }

        [Name("network_port_name_2")]
        public string NetworkPortName2 { get; set; }

        [Name("network_port_name_3")]
        public string NetworkPortName3 { get; set; }

        [Name("network_port_name_4")]
        public string NetworkPortName4 { get; set; }
    }
}