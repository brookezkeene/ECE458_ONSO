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

        [Name("offline_site")]
        public string OfflineSiteAbbreviation { get; set; }

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

        [Name("chassis_number")]
        public int? ChassisAssetNumber { get; set; }
        
        [Name("chassis_slot")]
        public int? ChassisSlot { get; set; }

        [Name("custom_display_color")]
        public string CustomDisplayColor { get; set; }

        [Name("custom_cpu")]
        public string CustomCpu { get; set; }
        
        [Name("custom_memory")]
        public string CustomMemory { get; set; }
        
        [Name("custom_storage")]
        public string CustomStorage { get; set; }

    }
}
