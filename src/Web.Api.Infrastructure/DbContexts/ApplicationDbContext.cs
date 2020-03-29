using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Interfaces;

namespace Web.Api.Infrastructure.DbContexts
{
    public class ApplicationDbContext : IdentityDbContext<User>, IApplicationDbContext
    {
        private const string AssetNumberSequenceName = "AssetNumberSequence";

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Model>()
                .HasIndex(m => new {m.Vendor, m.ModelNumber})
                .IsUnique();

            builder.Entity<ModelNetworkPort>()
                .HasIndex(o => new {o.ModelId, o.Number})
                .IsUnique();
            builder.Entity<ModelNetworkPort>()
                .HasIndex(o => new {o.ModelId, o.Name})
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
                .HasIndex(r => new {r.DatacenterId, r.Row, r.Column})
                .IsUnique();

            builder.Entity<Datacenter>()
                .HasIndex(dc => dc.Name)
                .IsUnique();
        }

        public DbSet<Model> Models { get; set; }
        public DbSet<Rack> Racks { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<Datacenter> Datacenters { get; set; }
        public DbSet<AssetNetworkPort> AssetNetworkPort { get; set; }
        public DbSet<AssetPowerPort> AssetPowerPort { get; set; }
        public DbSet<ImportFile> ImportFiles { get; set; }
        public DbSet<DecommissionedAsset> DecommissionedAssets { get; set; }
        public DbSet<ChangePlan> ChangePlans { get; set; }
        public DbSet<ChangePlanItem> ChangePlanItems { get; set; }
        public DbSet<NetworkConnection> NetworkConnections { get; set; }
        public DbSet<PowerConnection> PowerConnections { get; set; }
        public DbSet<PduPort> PduPort { get; set; }

        public Task<int> SaveChangesAsync()
        {
            var assetEntries = ChangeTracker
                .Entries<Asset>()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Cast<EntityEntry>();

            var changePlanEntries = ChangeTracker
                .Entries<ChangePlan>()
                .Where(e => e.State == EntityState.Added)
                .Cast<EntityEntry>();

            var changePlanItemEntries = ChangeTracker
                .Entries<ChangePlanItem>()
                .Where(e => e.State == EntityState.Added)
                .Cast<EntityEntry>();

            var entries = assetEntries
                .Concat(changePlanEntries)
                .Concat(changePlanItemEntries)
                .ToArray();

            foreach (var entry in entries)
            {
                switch (entry.Entity)
                {
                    case Asset asset:
                        asset.LastUpdatedDate = DateTime.Now;
                        break;
                    case ChangePlan plan:
                        plan.CreatedDate = DateTime.Now;
                        break;
                    case ChangePlanItem item:
                        item.CreatedDate = DateTime.Now;
                        break;
                }
            }

            return base.SaveChangesAsync();
        }
    }
}
