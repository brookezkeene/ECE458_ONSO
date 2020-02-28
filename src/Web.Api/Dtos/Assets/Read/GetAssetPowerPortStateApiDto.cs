using Web.Api.Core.Dtos.Power;

namespace Web.Api.Dtos.Assets.Read
{
    public class GetAssetPowerPortStateApiDto
    {
        public string Port { get; set; }
        public PowerState Status { get; set; }
    }
}