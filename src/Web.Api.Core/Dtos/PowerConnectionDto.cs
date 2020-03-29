using System;
using System.Collections.Generic;

namespace Web.Api.Core.Dtos
{
    public class PowerConnectionDto
    {
        public Guid Id { get; set; }
        public List<PowerPortDto> Ports { get; set; }
    }
}