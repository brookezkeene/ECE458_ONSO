using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Dtos.Assets.Read;

namespace Web.Api.Dtos.Assets.Create
{
    public class CreateDecommissionedAsset: GetAssetApiDto 
    {
        public string NetworkPortGraph { get; set; }
        public string Decommissioner { get; set; }
        public DateTime DateDecommissioned { get; set; }
    }

}
