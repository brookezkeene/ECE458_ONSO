using System;
using System.Collections.Generic;
using Web.Api.Dtos.Assets.Update;

namespace Web.Api.Dtos
{
    public class CreateNetworkConnectionApiDto
    {
        public List<CreateNetworkConnectionPortApiDto> Ports { get; set; }
    }

    public class CreateNetworkConnectionPortApiDto
    {
        public Guid Id { get; set; }
        public Guid ModelNetworkPortId { get; set; }
        public string MacAddress { get; set; }
    }
}