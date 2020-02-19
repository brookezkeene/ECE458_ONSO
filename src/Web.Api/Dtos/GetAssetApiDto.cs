using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Dtos
{
    public class GetAssetApiDto
    {
        public Guid Id { get; set; }
        public string Hostname { get; set; }
        public Guid RackId { get; set; }
        public string Rack { get; set; }
        public int RackPosition { get; set; }
        public int Height { get; set; }
        public IEnumerable<int> SlotsOccupied { get; set; }
        public string Comment { get; set; }
        public string Vendor { get; set; }
        public string ModelNumber { get; set; }
        public string DisplayColor { get; set; }
        public Guid ModelId { get; set; }
        public Guid? OwnerId { get; set; }
        public string Owner { get; set; }
    }
}
