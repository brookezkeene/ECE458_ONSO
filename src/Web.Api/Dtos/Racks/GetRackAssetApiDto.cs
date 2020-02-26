using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Api.Dtos.Models.Read;

namespace Web.Api.Dtos.Racks
{
    public class GetRackAssetApiDto
    {
        public Guid ModelId { get; set; }
        public GetModelApiDto Model { get; set; }
        public Guid Id { get; set; }
        public int RackPosition { get; set; }
        public IEnumerable<int> SlotsOccupied => Enumerable.Range(RackPosition, Model.Height);
        public int? AssetNumber { get; set; }
    }
}
