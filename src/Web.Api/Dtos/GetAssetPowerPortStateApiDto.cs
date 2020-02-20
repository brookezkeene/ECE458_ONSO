using Web.Api.Core.Dtos.Power;

namespace Web.Api.Dtos
{
    public class GetAssetPowerPortStateApiDto : GetAssetPowerPortApiDto
    {
        public PowerState Status { get; set; }
    }
}