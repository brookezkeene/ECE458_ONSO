using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IInstanceService
    {
        Task<PagedList<AssetDto>> GetInstancesAsync(string search, int page = 1, int pageSize = 10);
        Task<AssetDto> GetInstanceAsync(Guid instanceId);
        Task<List<AssetDto>> GetInstanceExportAsync(InstanceExportQuery query);
        Task<Guid> CreateInstanceAsync(AssetDto assetDto);
        Task DeleteInstanceAsync(Guid instanceId);
        Task<int> UpdateInstanceAsync(AssetDto assetDto);

    }
}