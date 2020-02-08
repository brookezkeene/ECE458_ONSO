using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web.Api.Core.Dtos
{
    public class ExportQuery
    {
        [Required]
        public string Search { get; set; }

    }
    public static class ExportQueryExtensions
    {
        public static ExportQuery ToUpper(this ExportQuery query)
        {
            var upperQuery = new ExportQuery { };
            /*upperQuery.StartRow = query.StartRow.ToUpper();
            upperQuery.EndRow = query.EndRow.ToUpper();*/
            upperQuery.Search = query.Search.ToUpper();
            return upperQuery;
        }
    }
}
