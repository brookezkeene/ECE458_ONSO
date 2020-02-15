using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Validation.Extensions
{
    public static class RackValidationExtensions
    {
        public static bool CanFitAllInstances(this Rack rack)
        {
            for (var i = 1; i <= 42; i++)
            {
                var slot = i;
                var instancesInSlot = rack.Instances.Where(instance =>
                    instance.RackPosition <= slot && slot < instance.RackPosition + instance.Model.Height);
                if (instancesInSlot.Count() > 1)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
