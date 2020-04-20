using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IDatacenterService
    {
        Task<PagedList<DatacenterDto>> GetDatacentersAsync(string search, int page = 1, int pageSize = 10);
        Task<DatacenterDto> GetDatacenterAsync(Guid datacenterId);

        Task<List<AssetNetworkPortDto>> GetNetworkPortsOfDataCenterAsync(Guid datacenterId);

        Task<int> UpdateDatacenterAsync(DatacenterDto datacenterDto);
        Task<Guid> CreateDatacenterAsync(DatacenterDto datacenterDto);
        Task DeleteDatacenterAsync(Guid datacenterId);
        Task<List<AssetNetworkPortDto>> GetOpenNetworkPortsOfDatacenterAsync(Guid datacenterId);
        Task<List<AssetDto>> GetChassisOfDataCenterAsync(Guid datacenterId);
        Task<PagedList<DatacenterDto>> GetOfflineDatacentersAsync(string search, int page = 1, int pageSize = 10);
     }
}