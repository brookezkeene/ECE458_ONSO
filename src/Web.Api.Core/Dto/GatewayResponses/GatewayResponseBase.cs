using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dto.GatewayResponses
{
    public class GatewayResponseBase
    {
        public bool Succeeded { get; }
        public IEnumerable<Error> Errors { get; }

        protected GatewayResponseBase(bool succeeded)
        {
            Succeeded = succeeded;
            Errors = new Error[0];
        }

        protected GatewayResponseBase(params Error[] errors) : this((IEnumerable<Error>)errors) { }

        protected GatewayResponseBase(IEnumerable<Error> errors)
        {
            if (errors == null)
            {
                errors = new[] {Error.Default};
            }

            Succeeded = false;
            Errors = errors;
        }

        public static GatewayResponseBase Success { get; } = new GatewayResponseBase(true);
    }
}
