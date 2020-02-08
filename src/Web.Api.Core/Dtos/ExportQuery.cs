using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web.Api.Core.Dtos
{
    public class ExportQuery : RackRangeQuery
    {
        [Required]
        public string Search { get; set; }

    }
    public static class ExportQueryExtensions
    {
        public static ExportQuery ToUpper(this ExportQuery query)
        {
            var upperQuery = new ExportQuery { StartCol = query.StartCol, EndCol = query.EndCol };
            upperQuery.StartRow = query.StartRow.ToUpper();
            upperQuery.EndRow = query.EndRow.ToUpper();
            upperQuery.Search = query.Search.ToUpper();
            return upperQuery;
        }
    }
}
