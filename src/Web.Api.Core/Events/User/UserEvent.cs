using Skoruba.AuditLogging.Events;

namespace Web.Api.Core.Events.User
{
    public abstract class UserEvent : AuditEvent
    {
        protected UserEvent() => Category = "User";
    }
}