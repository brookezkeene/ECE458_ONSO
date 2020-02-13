using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dtos
{
    public class ExportInstanceDto
    {
        public string hostname { get; set; }
        public string rack { get; set; }
        public int rack_position { get; set; }
        public string vendor { get; set; }
        public string model_number { get; set; }
        public string owner { get; set; }
        public string comment { get; set; }

    }
}
