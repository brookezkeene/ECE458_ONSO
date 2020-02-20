using System;
using System.Collections.Generic;

namespace Web.Api.Core.Dtos
{
    public class ModelDto
    {
        public ModelDto()
        {
            Assets = new List<AssetDto>();
        }
        public ModelDto(Guid id)
        {
            Id = id;
            Assets = new List<AssetDto>();
        }
        public List<AssetDto> Assets { get; set; }
        public Guid Id { get; set; }
        public string Vendor { get; set; }
        public string ModelNumber { get; set; }
        public int Height { get; set; }
        public string DisplayColor { get; set; }
        public int? EthernetPorts { get; set; }
        public int? PowerPorts { get; set; }
        public string Cpu { get; set; }
        public int? Memory { get; set; }
        public string Storage { get; set; }
        public string Comment { get; set; }
        public List<ModelNetworkPortDto> NetworkPorts { get; set; }
    }

    public class ModelNetworkPortDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid ModelId { get; set; }
        public ModelDto Model { get; set; }
    }
}