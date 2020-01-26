using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Web.Api.Infrastructure.Data.Ef.Repositories;
using Web.Api.Infrastructure.Data.Entities;

namespace Web.Api.Infrastructure.Data.Ef
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
    }
}