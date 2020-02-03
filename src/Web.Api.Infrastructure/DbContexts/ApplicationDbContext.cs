using Microsoft.EntityFrameworkCore;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Model>()
                .HasIndex(m => new {m.Vendor, m.ModelNumber})
                .IsUnique();

            builder.Entity<Instance>()
                .HasIndex(i => i.Hostname)
                .IsUnique();

            builder.Entity<Rack>()
                .HasIndex(r => new {r.Row, r.Column})
                .IsUnique();
        }

        public DbSet<Model> Models { get; set; }
        public DbSet<Rack> Racks { get; set; }
        public DbSet<Instance> Instances { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
