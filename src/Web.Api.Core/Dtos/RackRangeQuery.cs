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
        public int StartCol { get; set; }
        [Required]
        public string EndRow { get; set; }
        [Required]
        public int EndCol { get; set; }
        [Required]
        public Guid datacenterId { get; set; }
    }
    public class RackRangeDatacenterDto
    {
        [Required]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public static class RackRangeQueryExtensions
    {
        public static RackRangeQuery ToUpper(this RackRangeQuery query)
        {
            var upperQuery = new RackRangeQuery {StartCol = query.StartCol, EndCol = query.EndCol};
            upperQuery.StartRow = query.StartRow.ToUpper();
            upperQuery.EndRow = query.EndRow.ToUpper();
            upperQuery.datacenterId = query.datacenterId;
            return upperQuery;
        }
    }
}
