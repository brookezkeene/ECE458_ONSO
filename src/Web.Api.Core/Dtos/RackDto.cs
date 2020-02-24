using System;
using System.Collections.Generic;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Dtos
{
    public class RackDto
    {
        public List<AssetDto> Assets { get; set; }
        public Guid Id { get; set; }
        public string RowLetter { get; set; }
        public int RackNumber { get; set; }
        public string Address => $"{RowLetter}{RackNumber}";
        public Guid DatacenterId { get; set; }
        public DatacenterDto Datacenter { get; set; }
        public List<PduDto> Pdus { get; set; }
    }

    public class DatacenterDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasNetworkManagedPower { get; set; }
        public List<RackDto> Racks { get; set; }
    }
}