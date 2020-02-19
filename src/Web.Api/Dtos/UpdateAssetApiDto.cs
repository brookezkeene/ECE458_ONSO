using System;

namespace Web.Api.Dtos
{
    public class UpdateAssetApiDto
    {
        public Guid Id { get; set; }
        public Guid RackId { get; set; }
        public Guid OwnerId { get; set; }
        public Guid ModelId { get; set; }
        public string Hostname { get; set; }
        public string Comment { get; set; }
        public int RackPosition { get; set; }
    }
}