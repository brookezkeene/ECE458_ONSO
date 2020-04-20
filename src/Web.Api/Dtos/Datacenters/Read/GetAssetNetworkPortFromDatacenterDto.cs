using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Dtos.Datacenters.Read
{
    public class GetAssetNetworkPortFromDatacenterDto
    {
        public string AssetHostname { get; set; }
        public string AssetNumber { get; set; }
        public string RowLetter { get; set; }
        public int RackNumber { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string MacAddress { get; set; }
        public Guid ModelNetworkPortId { get; set; }
    }
}
