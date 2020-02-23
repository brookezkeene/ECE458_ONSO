using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Web.Api.Common
{
    [JsonObject(MemberSerialization.OptIn)]
    public class PagedList<T> : List<T> where T : class
    {
        [JsonProperty]
        public int TotalCount { get; set; }

        [JsonProperty]
        public int PageSize { get; set; }

        [JsonProperty]
        public int TotalPages => PageSize > 0 ? (TotalCount - 1) / PageSize + 1 : 0;

        [JsonProperty]
        public int CurrentPage { get; set; }

        [JsonProperty]
        public int? PrevPage => CurrentPage > 1 ? CurrentPage - 1 : (int?)null;

        [JsonProperty]
        public int? NextPage => CurrentPage < TotalPages ? CurrentPage + 1 : (int?)null;

        [JsonProperty]
        public T[] Data
        {
            get => ToArray();
            set
            {
                if (value != null)
                    AddRange(value);
            }
        }
    }
}
