using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        private const string AssetNumberSequenceName = "AssetNumberSequence";
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Model>()
                .HasIndex(m => new {m.Vendor, m.ModelNumber})
                .IsUnique();

            // auto-generate asset number
            builder.HasSequence<int>(AssetNumberSequenceName)
                .StartsAt(100000)
                .HasMax(999999)
                .IncrementsBy(1);

            builder.Entity<Asset>()
                .Property(i => i.AssetNumber)
                .HasDefaultValueSql($"NEXT VALUE FOR {AssetNumberSequenceName}");

            builder.Entity<Asset>()
                .HasIndex(i => i.AssetNumber)
                .IsUnique();

            builder.Entity<Rack>()
                .HasIndex(r => new {r.Row, r.Column})
                .IsUnique();

            builder.Entity<Datacenter>()
                .HasIndex(dc => dc.Name)
                .IsUnique();

            builder.Entity<AssetPowerPort>()
                .HasOne(o => o.PduPort)
                .WithOne(o => o.AssetPowerPort)
                .HasForeignKey<PduPort>(o => o.AssetPowerPortId);
        }

        public DbSet<Model> Models { get; set; }
        public DbSet<Rack> Racks { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Datacenter> Datacenters { get; set; }
    }
}
