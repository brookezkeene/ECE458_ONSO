using System;
using System.Collections.Generic;

namespace Web.Api.Dtos.Assets.Create
{
    public class CreateAssetApiDto : AssetApiDto
    {
        public Guid? ChangePlanId { get; set; }
        public List<CreateAssetPowerPortApiDto> PowerPorts { get; set; }
        public List<CreateAssetNetworkPortApiDto> NetworkPorts { get; set; }
    }
}
