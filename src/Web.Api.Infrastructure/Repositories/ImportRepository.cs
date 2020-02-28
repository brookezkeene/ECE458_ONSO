using System;
using System.Threading.Tasks;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Infrastructure.Repositories
{
    public class ImportRepository : IImportRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ImportRepository(ApplicationDbContext dbContext)
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