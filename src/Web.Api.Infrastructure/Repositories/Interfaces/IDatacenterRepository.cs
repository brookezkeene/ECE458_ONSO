using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IDatacenterRepository
    {
        Task<PagedList<Datacenter>> GetDatacentersAsync(string search, int page = 1, int pageSize = 10);
        Task<Datacenter> GetDatacenterAsync(Guid datacenterId);
        Task<int> AddDatacenterAsync(Datacenter datacenter);
        Task<int> UpdateDatacenterAsync(Datacenter datacenter);
        Task<int> DeleteDatacenterAsync(Datacenter datacenter);
        Task<bool> CanDeleteDatacenterAsync(Datacenter datacenter);
        Task<bool> RacksInDatacenterExistAsync(Datacenter datacenter);
        Task<List<AssetNetworkPort>> GetNetworkPortFromDatacenterAsync(Guid datacenterID);

        Task<bool> DatacenterIsUniqueAsync(string vendor, string modelNumber, Guid id = default);
        Task<bool> DatacenterExistsAsync(string vendor, string modelNumber, Guid id);

        Datacenter GetDatacenter(string name);
        Task<List<AssetNetworkPort>> GetOpenNetworkPortsFromDatacenterAsync(Guid datacenterId);
        Task<List<Asset>> GetChassisFromDatacenterAsync(Guid datacenterId);
    }
}
