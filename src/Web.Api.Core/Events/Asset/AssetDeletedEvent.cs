using Web.Api.Core.Dtos;

namespace Web.Api.Core.Events.Asset
{
    public class AssetDeletedEvent : AssetEvent
    {
        public AssetDto Asset { get; set; }

        public AssetDeletedEvent(AssetDto asset)
        {
            Asset = asset;
        }
    }
}