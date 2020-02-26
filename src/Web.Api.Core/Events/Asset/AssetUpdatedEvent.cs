using Web.Api.Core.Dtos;

namespace Web.Api.Core.Events.Asset
{
    public class AssetUpdatedEvent : AssetEvent
    {
        public AssetDto Asset { get; set; }

        public AssetUpdatedEvent(AssetDto asset)
        {
            Asset = asset;
        }
    }
}