using System.Threading.Tasks;
using Skoruba.AuditLogging.EntityFramework.Entities;
using Skoruba.AuditLogging.EntityFramework.Repositories;
using Web.Api.Common;
using Web.Api.Core.Dtos;
using Web.Api.Core.Mappers;
using Web.Api.Core.Services.Interfaces;

namespace Web.Api.Core.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly IAuditLoggingRepository<AuditLog> _repository;

        public AuditLogService(IAuditLoggingRepository<AuditLog> repository)
        {
            _repository = repository;
        }

        public async Task<PagedList<AuditLogDto>> GetAsync(string search, int page = 1, int pageSize = 10)
        {
            var list = await _repository.GetAsync(page, pageSize);

            var pagedList = list.MapTo<PagedList<AuditLogDto>>();
            pagedList.CurrentPage = page;

            return pagedList;
        }
    }
}