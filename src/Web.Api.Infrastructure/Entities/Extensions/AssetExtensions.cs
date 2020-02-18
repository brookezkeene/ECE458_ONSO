using System.Collections.Generic;
using System.Linq;

namespace Web.Api.Infrastructure.Entities.Extensions
{
    public static class AssetExtensions
    {
        public static IEnumerable<int> SlotsOccupied(this Asset asset)
        {
            return Enumerable.Range(asset.RackPosition, asset.Model.Height);
        }

        public static bool ConflictsWith(this Asset asset, Asset other)
        {
            return asset.SlotsOccupied().ToHashSet().Overlaps(other.SlotsOccupied());
        }
    }
}
