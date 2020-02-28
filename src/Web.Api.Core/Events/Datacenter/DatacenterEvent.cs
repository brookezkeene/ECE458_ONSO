using Skoruba.AuditLogging.Events;

namespace Web.Api.Core.Events.Datacenter
{
    public abstract class DatacenterEvent : AuditEvent
    {
        protected DatacenterEvent() => Category = "Datacenter";
    }
}