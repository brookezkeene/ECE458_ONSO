using System;

namespace Web.Api.Dtos.Assets
{
    public abstract class AssetApiDto
    {
        public Guid RackId { get; set; }
        public string Hostname { get; set; }
        public string Comment { get; set; }
        public int RackPosition { get; set; }
        public DateTime LastUpdatedDate { get; set; }

        public Guid? OwnerId { get; set; }
        public Guid ModelId { get; set; }
        public int? AssetNumber { get; set; }
    }
}