using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper.Configuration.Attributes;

namespace Web.Api.Core.Dtos
{
    public class ImportNetworkConnectionDto
    {
        [Name("src_hostname")]
        public string SourceHostname { get; set; }

        [Name("src_port")]
        public string SourcePortName { get; set; }

        [Name("src_mac")]
        public string SourceMacAddress { get; set; }

        [Name("dest_hostname")]
        public string DestinationHostname { get; set; }

        [Name("dest_port")]
        public string DestinationPortName { get; set; }
    }
}
