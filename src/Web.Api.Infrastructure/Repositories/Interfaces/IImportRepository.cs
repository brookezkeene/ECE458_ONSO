using System;
using System.Threading.Tasks;
using Web.Api.Infrastructure.Entities;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public interface IImportRepository
    {
        Task<Guid> AddImportAsync(string data);
        Task<ImportFile> GetImportAsync(Guid id);
    }
}