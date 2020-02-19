using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Validation.Extensions
{
    public static class RackValidationExtensions
    {
        public static bool CanFitAllAssets(this Rack rack)
        {
            for (var i = 1; i <= 42; i++)
            {
                var slot = i;
                var assetsInSlot = rack.Assets.Where(asset =>
                    asset.RackPosition <= slot && slot < asset.RackPosition + asset.Model.Height);
                if (assetsInSlot.Count() > 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
