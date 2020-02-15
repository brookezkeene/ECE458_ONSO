using System.Collections.Generic;
using System.Linq;

namespace Web.Api.Infrastructure.Entities.Extensions
{
    public static class InstanceExtensions
    {
        public static IEnumerable<int> SlotsOccupied(this Instance instance)
        {
            return Enumerable.Range(instance.RackPosition, instance.Model.Height);
        }

        public static bool ConflictsWith(this Instance instance, Instance other)
        {
            return instance.SlotsOccupied().ToHashSet().Overlaps(other.SlotsOccupied());
        }
    }
}
