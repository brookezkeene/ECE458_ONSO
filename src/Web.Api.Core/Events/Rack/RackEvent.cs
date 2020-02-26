using Skoruba.AuditLogging.Events;

namespace Web.Api.Core.Events.Rack
{
    public abstract class RackEvent : AuditEvent
    {
        protected RackEvent() => Category = "Rack";
    }
}