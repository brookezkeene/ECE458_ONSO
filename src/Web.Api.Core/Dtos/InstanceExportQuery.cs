using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web.Api.Core.Dtos
{
    public class InstanceExportQuery
    {
        [Required]
        public string Search { get; set; }
        public string Hostname { get; set; }
        public string StartRow { get; set; }
        public int StartCol { get; set; }
        public string EndRow { get; set; }
        public int EndCol { get; set; }
    }
    public static class InstanceExportQueryExtensions
    {
        public static InstanceExportQuery ReformatQuery(this InstanceExportQuery query)
        {
            var upperQuery = new InstanceExportQuery { };
            if (!string.IsNullOrEmpty(query.StartRow))
            {
                upperQuery.StartRow = query.StartRow.ToUpper();
                upperQuery.StartCol = query.StartCol;
            }
            else {
                upperQuery.StartRow = "A";
                upperQuery.StartCol = 1;

            }
            if (!string.IsNullOrEmpty(query.EndRow))
            {
                upperQuery.EndRow = query.EndRow.ToUpper();
                upperQuery.EndCol = query.EndCol;
            }
            else
            {
                upperQuery.EndRow = "Z";
                upperQuery.EndCol = int.MaxValue;

            }
            if (!string.IsNullOrEmpty(query.Search))
            {
                upperQuery.Search = query.Search.ToUpper();
            }
            if (!string.IsNullOrEmpty(query.Hostname))
            {
                upperQuery.Hostname = query.Hostname.ToUpper();
            }

            return upperQuery;
        }
    }
}
