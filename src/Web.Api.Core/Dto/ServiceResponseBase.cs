using System.Collections.Generic;

namespace Web.Api.Core.Dto
{
    public abstract class ServiceResponseBase
    {
        public bool Succeeded { get; }
        public IEnumerable<Error> Errors { get; }

        protected ServiceResponseBase(bool succeeded)
        {
            Succeeded = succeeded;
            Errors = new Error[0];
        }

        protected ServiceResponseBase(params Error[] errors) : this((IEnumerable<Error>) errors) { }

        protected ServiceResponseBase(IEnumerable<Error> errors)
        {
            if (errors == null)
            {
                errors = new[] {Error.Default};
            }

            Succeeded = false;
            Errors = errors;
        }

    }
}