using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;

        public AuditLogService(IAuditLoggingRepository<AuditLog> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PagedList<AuditLogDto>> GetAsync(string search, int page = 1, int pageSize = 10)
        {
            var list = await _repository.GetAsync(page, pageSize);

            var pagedList = _mapper.Map<PagedList<AuditLogDto>>(list);
            pagedList.CurrentPage = page;

            return pagedList;
        }
    }
}