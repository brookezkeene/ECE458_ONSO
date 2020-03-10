using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dtos
{
    public class DecommissionedAssetQuery
    {
        public string NetworkPortGraph { get; set; }
        public Guid Id { get; set; }
        public string TimeStamp { get; set; }
        public string Decommissioner { get; set; }
    }

}
