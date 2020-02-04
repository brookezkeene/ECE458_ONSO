using System;
using System.Collections.Generic;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Dtos
{
    public class RackDto
    {
        public RackDto()
        {
            Instances = new List<InstanceDto>();
        }

        public Guid Id { get; set; }
        public string RowLetter { get; set; }
        public int RackNumber { get; set; }
        public string Address => $"{RowLetter}{RackNumber}";
        public List<InstanceDto> Instances { get; set; }
    }
}