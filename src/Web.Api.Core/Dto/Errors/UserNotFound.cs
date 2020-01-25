using System;
using System.Collections.Generic;
using System.Text;

namespace Web.Api.Core.Dto.Errors
{
    public class UserNotFound : Error
    {
        public UserNotFound(string description = null) : base(description) { }
    }
}
