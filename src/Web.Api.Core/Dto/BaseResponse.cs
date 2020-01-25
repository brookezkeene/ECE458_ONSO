using System.Collections.Generic;

namespace Web.Api.Core.Dto
{
    public abstract class BaseResponse
    {
        public bool Success { get; }
        public IEnumerable<Error> Errors { get; }

        protected BaseResponse(bool success, IEnumerable<Error> errors)
        {
            Success = success;
            Errors = errors;
        }
    }
}