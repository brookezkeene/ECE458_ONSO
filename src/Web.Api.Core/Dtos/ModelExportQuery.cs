using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web.Api.Core.Dtos
{
    public class ModelExportQuery
    {
        public string Search { get; set; }

    }
    public static class ModelExportQueryExtensions
    {
        public static ModelExportQuery ToUpper(this ModelExportQuery query)
        {
            var upperQuery = new ModelExportQuery { };
            /*upperQuery.StartRow = query.StartRow.ToUpper();
            upperQuery.EndRow = query.EndRow.ToUpper();*/
            upperQuery.Search = query.Search.ToUpper();
            return upperQuery;
        }
    }
}
