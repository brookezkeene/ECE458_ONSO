using System;
using System.Collections.Generic;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Dtos
{
    public class PduDto
    {
        public Guid Id { get; set; }
        public int NumPorts { get; set; } = 24;
        public List<PduPortDto> Ports { get; set; }
        public PduLocation Location { get; set; }
        public Guid RackId { get; set; }
        public RackDto Rack { get; set; }

        public override string ToString()
        {
            var datacenter = Rack.Datacenter.Name.ToLower();
            var rack = $"{Rack.RowLetter}{Rack.RackNumber:00}";

            return $"hpdu-{datacenter}-{rack}{Location}";
        }

        public string ToPdu()
        {
            var datacenter = Rack.Datacenter.Name.ToLower();
            var rack = Rack.RackAddress.ToUpper();

            return $"{datacenter}-{rack}{Location}";
        }
    }
}