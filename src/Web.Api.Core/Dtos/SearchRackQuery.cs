using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Core.Dtos
{
    public class SearchRackQuery
    {
        public Guid? Datacenter { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string IsDesc { get; set; }
        public string SortBy { get; set; }
    }
    public static class SearchRackQueryExtensions
    {
        public static void ToUpper(this SearchRackQuery query)
        {
            if (query.Page == 0)
            {
                query.Page = 1;
            }
            if (query.PageSize == 0)
            {
                query.PageSize = 10;
            }

        }
    }
}
