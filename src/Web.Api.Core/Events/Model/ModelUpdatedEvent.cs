using Web.Api.Core.Dtos;

namespace Web.Api.Core.Events.Model
{
    public class ModelUpdatedEvent : ModelEvent
    {
        public ModelDto Model { get; set; }

        public ModelUpdatedEvent(ModelDto model)
        {
            Model = model;
        }
    }
}