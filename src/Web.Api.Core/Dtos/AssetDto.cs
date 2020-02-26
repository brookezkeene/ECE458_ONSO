using System;
using System.Collections.Generic;
using System.Linq;
using Web.Api.Core.Dtos.Power;
using Web.Api.Infrastructure.Entities;

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
        public Guid Id { get; set; }
        public string Hostname { get; set; }
        public int RackPosition { get; set; }
        public string Comment { get; set; }
        public IEnumerable<int> SlotsOccupied => Enumerable.Range(RackPosition, Model.Height);
        public int? AssetNumber { get; set; }
        public List<AssetPowerPortDto> PowerPorts { get; set; }
        public List<AssetNetworkPortDto> NetworkPorts { get; set; }
    }

    public class AssetNetworkPortDto
    {
        public Guid Id { get; set; }
        public string MacAddress { get; set; }
        public Guid AssetId { get; set; }
        public AssetDto Asset { get; set; }
        public Guid ModelNetworkPortId { get; set; }
        public ModelNetworkPortDto ModelNetworkPort { get; set; }
        public Guid? ConnectedPortId { get; set; }
        public AssetNetworkPortDto ConnectedPort { get; set; }
    }

    public class AssetPowerPortDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public Guid AssetId { get; set; }
        public AssetDto Asset { get; set; }
        public Guid? PduPortId { get; set; }
        public PduPortDto PduPort { get; set; }
    }

    public class PduPortDto
    {
        public Guid Id { get; set; }
        public int Number { get; set; }
        public Guid PduId { get; set; }
        public PduDto Pdu { get; set; }
        public Guid? AssetPowerPortId { get; set; }
        public AssetPowerPortDto AssetPowerPort { get; set; }

        public override string ToString()
        {
            return $"{Pdu}{Number}";
        }
    }

    public class PduDto
    {
        public Guid Id { get; set; }
        public int NumPorts { get; set; } = 24;
        public List<PduPort> Ports { get; set; }
        public PduLocation Location { get; set; }
        public Guid RackId { get; set; }
        public RackDto Rack { get; set; }

        public override string ToString()
        {
            var datacenter = Rack.Datacenter.Name.ToLower();
            var rack = Rack.Address.ToUpper();

            return $"hpdu-{datacenter}-{rack}{Location}";
        }
    }

}