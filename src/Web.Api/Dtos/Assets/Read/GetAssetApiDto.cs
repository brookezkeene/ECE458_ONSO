using System.Collections.Generic;

namespace Web.Api.Dtos.Assets.Read
{
    public class GetAssetApiDto : GetAssetsApiDto
    {
        public IEnumerable<int> SlotsOccupied { get; set; }
        public List<GetAssetPowerPortApiDto> PowerPorts { get; set; }
        public List<GetAssetNetworkPortApiDto> NetworkPorts { get; set; } 
    }
}