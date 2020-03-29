using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Infrastructure.Repositories
{
    public class ImportRepository<TDbContext> : IImportRepository
        where TDbContext : DbContext, IApplicationDbContext
    {
        private readonly TDbContext _dbContext;

        public ImportRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> AddImportAsync(string data)
        {
            var importFile = new ImportFile
            {
                Data = data
            };

            await _dbContext.ImportFiles.AddAsync(importFile);
            await _dbContext.SaveChangesAsync();

            return importFile.Id;
        }

        public async Task<ImportFile> GetImportAsync(Guid id)
        {
            return await _dbContext.ImportFiles.FindAsync(id);
        }
    }
}