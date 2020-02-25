using System;
using System.Collections.Generic;

namespace Web.Api.Dtos.Users
{
    public class UpdateUserRolesApiDto
    {
        public List<string> Roles { get; set; }
    }
}
