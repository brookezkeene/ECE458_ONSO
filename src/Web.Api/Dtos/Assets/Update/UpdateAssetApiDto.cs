using System;
using System.Collections.Generic;

namespace Web.Api.Dtos.Assets.Update
{
    public class UpdateAssetApiDto : AssetApiDto
    {
        public Guid? ChangePlanId { get; set; }

        public List<UpdateAssetPowerPortApiDto> PowerPorts { get; set; }
        public List<UpdateAssetNetworkPortApiDto> NetworkPorts { get; set; }
        public Guid Id { get; set; }
    }
}