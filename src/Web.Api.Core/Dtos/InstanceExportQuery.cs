using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web.Api.Core.Dtos
{
    public class InstanceExportQuery:RackRangeQuery
    {
        [Required]
        public string Search { get; set; }
    }
    public static class InstanceExportQueryExtensions
    {
        public static InstanceExportQuery ToUpper(this InstanceExportQuery query)
        {      
            var upperQuery = ToUpper(query);
            return upperQuery;
        }
    }
}
