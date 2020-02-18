using System;
using System.Collections.Generic;
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
            //if 
            Expression<Func<Model, bool>> searchCondition = x => (x.ModelNumber.Contains(search) || x.Vendor.Contains(search));

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
        public async Task<List<Model>> GetModelExportAsync(string search)
        {
            Expression<Func<Model, bool>> searchCondition = x => (x.ModelNumber.ToUpper().Contains(search) || x.Vendor.ToUpper().Contains(search));

            var models = await _dbContext.Models
                .WhereIf(!string.IsNullOrEmpty(search), searchCondition)
                .AsNoTracking()
                .ToListAsync();
            //models = models.Where(x => x.ModelNumber.Contains(search) || x.Vendor.Contains(search)).ToList();
            return models;

        }
        public async Task<Model> GetModelAsync(Guid modelId)
        {
            return await _dbContext.Models
                .Include(x => x.Assets)
                .Where(x => x.Id == modelId)
                .AsNoTracking()
                .SingleAsync();
        }

        public async Task<Model> FindModel(Expression<Func<Model, bool>> expr)
        {
            return await _dbContext.Models
                .Where(expr)
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

        public async Task<bool> CanUpdateModelAsync(Model model)
        {
            return await MeetsHeightChangeCriteriaAsync(model) && await UniquenessConstraint(model);
        }

        public async Task<bool> AssetsOfModelExistAsync(Model model)
        {
            return await _dbContext.Assets.Where(x => x.Model == model)
                .AnyAsync();
        }

        public async Task<bool> ModelIsUniqueAsync(string vendor, string modelNumber, Guid id = default)
        {
            return !await _dbContext.Models
                .Where(x => x.Vendor == vendor && x.ModelNumber == modelNumber)
                .WhereIf(id != default, x => x.Id != id)
                .AnyAsync();
        }

        private async Task<bool> UniquenessConstraint(Model model)
        {
            // models are unique by vendor and model number
            Expression<Func<Model, bool>> uniquenessViolation = x =>
                x.Vendor == model.Vendor && x.ModelNumber == model.ModelNumber && x.Id != model.Id;
            var conflict = await FindModel(uniquenessViolation);
            return conflict == null;
        }

        public async Task<bool> MeetsHeightChangeCriteriaAsync(Model model)
        {
            if (model.Id != default)
            {
                var currentData = await GetModelAsync(model.Id);

                // disallow height change if model has assets
                if (model.Height == currentData.Height) return true;

                var hasAssets = await _dbContext.Assets.Where(x => x.Model == model)
                    .AnyAsync();

                return !hasAssets;
            }

            return true;
        }

        public async Task<bool> ModelExistsAsync(string vendor, string modelNumber, Guid id)
        {
            return await _dbContext.Models
                // lookup by id if we were given one
                .WhereIf(id != default, x => x.Id == id)
                // otherwise lookup by vendor & model number
                .WhereIf(id == default, x => x.Vendor == vendor && x.ModelNumber == modelNumber)
                .AnyAsync();
        }
    }
}
