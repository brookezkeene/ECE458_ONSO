using System;
using System.Collections.Generic;
using System.Linq;
using Web.Api.Core.Dtos.Power;

namespace Web.Api.Core.Dtos
{
    public class AssetDto
    {
        public Guid ModelId { get; set; }
        public ModelDto Model { get; set; }
        public Guid RackId { get; set; }
        public RackDto Rack { get; set; }
        public Guid? OwnerId { get; set; }
        public UserDto Owner { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public Guid Id { get; set; }
        public string Hostname { get; set; }
        public int RackPosition { get; set; }
        public string Comment { get; set; }
        public IEnumerable<int> SlotsOccupied => Model == null ? null : Enumerable.Range(RackPosition, Model.Height ?? 0);
        public int? AssetNumber { get; set; }
        public List<AssetPowerPortDto> PowerPorts { get; set; }
        public List<AssetNetworkPortDto> NetworkPorts { get; set; }
        public List<AssetDto> Blades { get; set; }
        public Guid? ChassisId { get; set; }
        public AssetDto Chassis { get; set; }
        public int? ChassisSlot { get; set; }
        public string CustomDisplayColor { get; set; }
        public string CustomCpu { get; set; }
        public int? CustomMemory { get; set; }
        public string CustomStorage { get; set; }
    }
}