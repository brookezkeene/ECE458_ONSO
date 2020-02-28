using Skoruba.AuditLogging.Events;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Events.Datacenter
{
    public class DatacenterDeletedEvent : DatacenterEvent
    {
        public DatacenterDto Datacenter { get; set; }

        public DatacenterDeletedEvent(DatacenterDto datacenter)
        {
            Datacenter = datacenter;
        }
    }
}