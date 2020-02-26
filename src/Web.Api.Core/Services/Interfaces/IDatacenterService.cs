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
        Task<int> UpdateDatacenterAsync(DatacenterDto datacenter);
        Task<Guid> CreateDatacenterAsync(DatacenterDto datacenter);
        Task DeleteDatacenterAsync(DatacenterDto datacenter);
    }
}