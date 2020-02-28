using Skoruba.AuditLogging.Events;

namespace Web.Api.Core.Events.Asset
{
    public abstract class AssetEvent : AuditEvent
    {
        protected AssetEvent() => Category = "Asset";
    }
}