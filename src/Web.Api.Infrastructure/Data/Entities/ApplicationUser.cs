using Microsoft.AspNetCore.Identity;
using Web.Api.Core.Domain.Entities;

namespace Web.Api.Infrastructure.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
    }
}