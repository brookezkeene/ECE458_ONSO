using System;
using System.Collections.Generic;

namespace Web.Api.Common
{
    public class PagedList<T> : List<T> where T : class
    {
        public int TotalCount { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => (TotalCount - 1) / PageSize + 1;

        public int CurrentPage { get; set; }

        public int? PrevPage => CurrentPage > 1 ? CurrentPage - 1 : (int?)null;

        public int? NextPage => CurrentPage < TotalPages ? CurrentPage + 1 : (int?) null;
    }
}
