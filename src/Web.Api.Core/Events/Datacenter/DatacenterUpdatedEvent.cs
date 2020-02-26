using Skoruba.AuditLogging.Events;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Events.Datacenter
{
    public class DatacenterUpdatedEvent : DatacenterEvent
    {
        public DatacenterDto Datacenter { get; set; }

        public DatacenterUpdatedEvent(DatacenterDto datacenter)
        {
            Datacenter = datacenter;
        }
    }
}