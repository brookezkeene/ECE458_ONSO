using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Web.Api.Core.Dtos
{
    public class RackRangeQuery
    {
        [Required]
        public string StartRow { get; set; }
        [Required]
        public string EndRow { get; set; }
        public int StartCol { get; set; }
        public int EndCol { get; set; }
        public Guid DatacenterId { get; set; }
    }

    public static class RackRangeQueryExtensions
    {
        public static RackRangeQuery ToUpper(this RackRangeQuery query)
        {
            var upperQuery = new RackRangeQuery {StartCol = query.StartCol, EndCol = query.EndCol};
            upperQuery.StartRow = query.StartRow.ToUpper();
            upperQuery.EndRow = query.EndRow.ToUpper();
            upperQuery.DatacenterId = query.DatacenterId;
            return upperQuery;
        }
    }
}
