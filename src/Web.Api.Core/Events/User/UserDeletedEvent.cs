using Skoruba.AuditLogging.Events;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Events.User
{
    public class UserDeletedEvent : UserEvent
    {
        public UserDto User { get; set; }

        public UserDeletedEvent(UserDto user)
        {
            User = user;
        }
    }
}