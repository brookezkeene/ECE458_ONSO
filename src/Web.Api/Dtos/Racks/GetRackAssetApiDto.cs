using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Core.Dtos;

namespace Web.Api.Dtos.Racks
{
    public class GetRackAssetApiDto
    {
        public string Vendor { get; set; }
        public string ModelNumber { get; set; }
        public string DisplayColor { get; set; }
        public Guid Id { get; set; }
        public string Hostname { get; set; }
        public string Owner { get; set; }
        public int Height { get; set; }
        public int RackPosition { get; set; }
        public IEnumerable<int> SlotsOccupied { get; set; }
        public int AssetNumber { get; set; }
        public string CustomDisplayColor { get; set; }
    }
}
