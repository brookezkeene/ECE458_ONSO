using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.ExceptionHandling
{
    public class UserFriendlyException : Exception
    {
        public string ErrorKey { get; set; }
        public object Object { get; set; }

        public UserFriendlyException(string message, string errorKey, object @object) : base(message)
        {
            ErrorKey = errorKey;
            Object = @object;
        }
    }
}
