using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Api.Common;
using Web.Api.Common.Extensions;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Infrastructure.Repositories
{
    public class ModelRepository : IModelRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ModelRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<PagedList<Model>> GetModelsAsync(string search, int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<Model>();
            Expression<Func<Model, bool>> searchCondition = x => x.ModelNumber.Contains(search);

            var models = await _dbContext.Models
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .PageBy(x => x.ModelNumber, page, pageSize)
                .AsNoTracking()
                .ToListAsync();

            pagedList.AddRange(models);
            pagedList.TotalCount = await _dbContext.Models
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.CurrentPage = page;

            return pagedList;
        }

        public async Task<Model> GetModelAsync(Guid modelId)
        {
            return await _dbContext.Models
                .Include(x => x.Instances)
                .Where(x => x.Id == modelId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<int> AddModelAsync(Model model)
        {
            _dbContext.Models.Add(model);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateModelAsync(Model model)
        {
            _dbContext.Models.Update(model);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteModelAsync(Model model)
        {
            _dbContext.Models.Remove(model);
            return await _dbContext.SaveChangesAsync();
        }
    }
}
