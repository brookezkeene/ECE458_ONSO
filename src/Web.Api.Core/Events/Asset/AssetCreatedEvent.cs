using Skoruba.AuditLogging.Events;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Events.Asset
{
    public class AssetCreatedEvent : AssetEvent
    {
        public AssetDto Asset { get; set; }

        public AssetCreatedEvent(AssetDto asset)
        {
            Asset = asset;
        }
    }
}