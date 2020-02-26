using Skoruba.AuditLogging.Events;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Events.Model
{
    public class ModelDeletedEvent : ModelEvent
    {
        public ModelDto Model { get; set; }

        public ModelDeletedEvent(ModelDto model)
        {
            Model = model;
        }
    }
}