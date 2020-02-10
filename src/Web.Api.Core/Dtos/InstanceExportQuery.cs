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
            var upperQuery = new InstanceExportQuery { };
            upperQuery.StartRow = query.StartRow.ToUpper();
            upperQuery.EndRow = query.EndRow.ToUpper();
            upperQuery.Search = query.Search.ToUpper();
            return upperQuery;
        }
    }
}
