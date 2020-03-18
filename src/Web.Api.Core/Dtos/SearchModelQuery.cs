using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Core.Dtos
{
    public class SearchModelQuery
    {
        public string Vendor { get; set; }
        public string Number { get; set; }
        public int HeightStart { get; set; }
        public int HeightEnd { get; set; }
        public int NetworkRangeStart { get; set; }
        public int NetworkRangeEnd { get; set; }
        public int PowerRangeStart { get; set; }
        public int PowerRangeEnd { get; set; }
        public int MemoryRangeStart { get; set; }
        public int MemoryRangeEnd { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
    public static class SearchModelQueryExtensions
    {
        public static void ToUpper(this SearchModelQuery query)
        {
            //setting defaults for model vendor and number search
            if (!string.IsNullOrEmpty(query.Vendor))
            {
                query.Vendor = query.Vendor.ToUpper();
            }
            else
            {
                query.Vendor = "";
            }
            if (!string.IsNullOrEmpty(query.Number))
            {
                query.Number = query.Number.ToUpper();
            }
            else
            {
                query.Number = "";
            }
            //if the query's end ranges are 0, default them to be the max possible value
            //assume that the user didn't enter number as the "end" range
            if(query.HeightEnd == 0)
            {
                query.HeightEnd = int.MaxValue;
            }
            if(query.NetworkRangeEnd == 0)
            {
                query.NetworkRangeEnd = int.MaxValue;
            }
            if(query.PowerRangeEnd == 0)
            {
                query.PowerRangeEnd = int.MaxValue;
            }
            if(query.MemoryRangeEnd == 0)
            {
                query.MemoryRangeEnd = int.MaxValue;
            }

        }
    }
}
