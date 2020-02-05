using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Resources
{
    public interface IModelServiceResources
    {
        ValidationError CannotChangeHeightIfInstancesExist();
        ValidationError GeneralConstraintViolation();
    }

    public class ModelServiceResources : IModelServiceResources
    {
        public ValidationError CannotChangeHeightIfInstancesExist()
        {
            return new ValidationError()
            {
                Code = nameof(CannotChangeHeightIfInstancesExist),
                Description = "Cannot update the height of a model with existing instances"
            };
        }

        public ValidationError GeneralConstraintViolation()
        {
            return new ValidationError()
            {
                Code = nameof(GeneralConstraintViolation),
                Description = "A data constraint would have been violated by this operation"
            };
        }
    }
}
