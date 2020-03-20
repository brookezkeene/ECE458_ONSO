using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dtos
{
    public class SearchAssetQuery
    {
        public Guid? Datacenter { get; set; }
        public string Vendor { get; set; }
        public string ModelNumber { get; set; }
        public string RackStart { get; set; }
        public string RackEnd { get; set; }
        public string Hostname { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string IsDesc { get; set; }
        public string SortBy { get; set; }
    }
    public static class SearchAssetQueryExtensions
    {
        public static void ToUpper(this SearchAssetQuery query)
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
            if (!string.IsNullOrEmpty(query.ModelNumber))
            {
                query.ModelNumber = query.ModelNumber.ToUpper();
            }
            else
            {
                query.ModelNumber = "";
            }
            if (!string.IsNullOrEmpty(query.Hostname))
            {
                query.Hostname = query.Hostname.ToUpper();
            }
            else
            {
                query.Hostname = "";
            }
            if (!string.IsNullOrEmpty(query.RackStart))
            {
                query.RackStart = query.RackStart.ToUpper();
            }
            else
            {
                query.RackStart = "A" + 0.ToString();
            }
            if (!string.IsNullOrEmpty(query.RackEnd))
            {
                query.RackEnd = query.RackEnd.ToUpper();
            } else if (query.RackEnd.Length == 1)
            {
                query.RackEnd = query.RackEnd.ToUpper() + int.MaxValue.ToString();
            }
            else
            {
                query.RackEnd = "Z" + int.MaxValue.ToString();
            }

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
