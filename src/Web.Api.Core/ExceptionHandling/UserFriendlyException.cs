using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.ExceptionHandling
{
    public class UserFriendlyException : Exception
    {
        public string ErrorKey { get; set; }

        protected UserFriendlyException(string message, string errorKey) : base(message)
        {
            ErrorKey = errorKey;
        }

        protected UserFriendlyException(string message, string errorKey, Exception innerException) : base(message,
            innerException)
        {
            ErrorKey = errorKey;
        }
    }
}
