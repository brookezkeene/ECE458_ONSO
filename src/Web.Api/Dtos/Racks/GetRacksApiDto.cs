using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Dtos;

namespace Web.Api.Dtos.Racks
{
    public class GetRacksApiDto
    {
        public List<GetRackAssetApiDto> Assets { get; set; }
        public Guid Id { get; set; }
        public string RowLetter { get; set; }
        public int RackNumber { get; set; }
        public string Address => $"{RowLetter}{RackNumber}";
        public Guid DatacenterId { get; set; }
        public RackDatacenterApiDto Datacenter { get; set; }
        public List<GetRackPdusApiDto> Pdus { get; set; }
    }
}
