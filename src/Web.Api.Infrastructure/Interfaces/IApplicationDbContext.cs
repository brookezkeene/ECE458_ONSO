using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Model> Models { get; set; }
        DbSet<Rack> Racks { get; set; }
        DbSet<Asset> Assets { get; set; }
        DbSet<Datacenter> Datacenters { get; set; }
        DbSet<AssetNetworkPort> AssetNetworkPort { get; set; }
        DbSet<AssetPowerPort> AssetPowerPort { get; set; }
        DbSet<ImportFile> ImportFiles { get; set; }
        DbSet<DecommissionedAsset> DecommissionedAssets { get; set; }
        DbSet<NetworkConnection> NetworkConnections { get; set; }
        DbSet<PowerConnection> PowerConnections { get; set; }
        DbSet<PduPort> PduPort { get; set; }
        DbSet<ChangePlan> ChangePlans { get; set; }
        DbSet<ChangePlanItem> ChangePlanItems { get; set; }
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
