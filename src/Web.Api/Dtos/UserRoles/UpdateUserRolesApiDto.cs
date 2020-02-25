using System;
using System.Collections.Generic;

namespace Web.Api.Dtos.Users
{
    public class UpdateUserRoleApiDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
