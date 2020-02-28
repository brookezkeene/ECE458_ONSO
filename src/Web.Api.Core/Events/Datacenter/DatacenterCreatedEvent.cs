using Web.Api.Core.Dtos;

namespace Web.Api.Core.Events.Datacenter
{
    public class DatacenterCreatedEvent : DatacenterEvent
    {
        public DatacenterDto Datacenter { get; set; }

        public DatacenterCreatedEvent(DatacenterDto datacenter)
        {
            Datacenter = datacenter;
        }
    }
}