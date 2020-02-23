using System.Threading.Tasks;
using Web.Api.Common;
using Web.Api.Core.Dtos;

namespace Web.Api.Core.Services.Interfaces
{
    public interface IAuditLogService
    {
        Task<PagedList<AuditLogDto>> GetAsync(string search, int page = 1, int pageSize = 10);
    }
}