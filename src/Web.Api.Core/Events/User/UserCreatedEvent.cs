using Web.Api.Core.Dtos;

namespace Web.Api.Core.Events.User
{
    public class UserCreatedEvent : UserEvent
    {
        public UserDto User { get; set; }

        public UserCreatedEvent(UserDto user)
        {
            User = user;
        }
    }
}