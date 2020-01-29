using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Web.Api.Infrastructure.Entities
{
    public class User : IdentityUser
    {
        [Required]
        public string DisplayName { get; set; }
    }
}
