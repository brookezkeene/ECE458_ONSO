using Skoruba.AuditLogging.Events;

namespace Web.Api.Core.Events.Model
{
    public abstract class ModelEvent : AuditEvent
    {
        protected ModelEvent() => Category = "Model";
    }
}