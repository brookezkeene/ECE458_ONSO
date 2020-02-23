using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;


namespace Web.Api.Core.Services
{
    public class DatacenterService : IDatacenterService
    {
        private readonly IDatacenterRepository _repository;

        public DatacenterService(IDatacenterRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> CreateDatacenterAsync(DatacenterDto datacenterDto)
        {
            var entity = datacenterDto.ToEntity();
            await _repository.AddDatacenterAsync(entity);
            return entity.Id;
        }

        public async Task DeleteDatacenterAsync(Guid datacenterId)
        {
            var entity = await _repository.GetDatacenterAsync(datacenterId);
            await _repository.DeleteDatacenterAsync(entity);
        }

        public async Task<DatacenterDto> GetDatacenterAsync(Guid datacenterId)
        {
            var datacenter = await _repository.GetDatacenterAsync(datacenterId);
            // TODO: handle null result (no datacenter found)
            return datacenter.ToDto();
        }

        public async Task<PagedList<DatacenterDto>> GetDatacentersAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = await _repository.GetDatacentersAsync(search, page, pageSize);
            return pagedList.ToDto();
        }

        public async Task<int> UpdateDatacenterAsync(DatacenterDto datacenterDto)
        {
            var datacenter = datacenterDto.ToEntity();
            var updated = await _repository.UpdateDatacenterAsync(datacenter);

            return updated;
        }
    }
}
