using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper.Configuration.Attributes;
using Web.Api.Core.Helpers;

namespace Web.Api.Core.Dtos
{
    public class ImportAssetDto : IRackAddressed
    {
        [Name("asset_number")]
        public int? AssetNumber { get; set; }

        [Name("hostname")]
        public string Hostname { get; set; }

        [Name("datacenter")]
        public string DatacenterAbbreviation { get; set; }

        [Name("rack")]
        public string RackAddress { get; set; }

        [Name("rack_position")]
        public int RackPosition { get; set; }

        [Name("vendor")]
        public string Vendor { get; set; }

        [Name("model_number")]
        public string ModelNumber { get; set; }

        [Name("owner")]
        public string OwnerUsername { get; set; }

        [Name("comment")]
        public string Comment { get; set; }

        [Name("power_port_connection_1")]
        public string PowerPortConnection1 { get; set; }

        [Name("power_port_connection_2")]
        public string PowerPortConnection2 { get; set; }
    }
}
