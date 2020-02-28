using System;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Events.Rack
{
    public class RackDeletedEvent : RackEvent
    {
        public RackRangeQuery Range { get; set; }

        public RackDeletedEvent(RackRangeQuery range)
        {
            Range = range;
        }
    }
}