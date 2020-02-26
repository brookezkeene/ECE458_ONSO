using System;
using Skoruba.AuditLogging.Events;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Events.Rack
{
    public class RackCreatedEvent : RackEvent
    {
        public RackRangeQuery Range { get; set; }

        public RackCreatedEvent(RackRangeQuery range)
        {
            Range = range;
        }
    }
}