using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Infrastructure.DbContexts;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Infrastructure.Repositories
{

    public class ChangePlanRepository : IChangePlanRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ChangePlanRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ChangePlan> GetChangePlanAsync(Guid changePlanId)
        {
            return await _dbContext.ChangePlans
                .Where(x => x.Id == changePlanId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }
        public async Task<ChangePlanItem> GetChangePlanItemAsync(Guid changePlanItemId)
        {
            return await _dbContext.ChangePlanItems
                .Where(x => x.Id == changePlanItemId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
        }
        public async Task<List<ChangePlan>> GetChangePlansAsync(Guid? createdById)
        {
            var changePlans = await _dbContext.ChangePlans
                .Where(x => x.Id == createdById)
                .AsNoTracking()
                .ToListAsync();
            return changePlans;
        }
        public async Task<List<ChangePlanItem>> GetChangePlanItemsAsync(Guid changePlanId)
        {
            var changePlans = await _dbContext.ChangePlanItems
                .Where(x => x.Id == changePlanId)
                .AsNoTracking()
                .ToListAsync();
            return changePlans;
        }
        public async Task<int> AddChangePlanAsync(ChangePlan changePlan)
        {
            _dbContext.ChangePlans.Add(changePlan);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> AddChangePlanItemAsync(ChangePlanItem changePlanItem)
        {
            _dbContext.ChangePlanItems.Add(changePlanItem);
            return await _dbContext.SaveChangesAsync();
        }
        //TODO: can you update a changePlan?
        public async Task<int> UpdateChangePlanItemAsync(ChangePlanItem changePlanItem)
        {
            _dbContext.ChangePlanItems.Update(changePlanItem);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteChangePlanAsync(ChangePlan changePlan)
        {
            //if a change plan is deleted, we need to also delete its items
            await DeleteChangePlanItemsAsync(changePlan.Id);
            _dbContext.ChangePlans.Remove(changePlan);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> DeleteChangePlanItemAsync(ChangePlanItem changePlanItem)
        {
            _dbContext.ChangePlanItems.Remove(changePlanItem);
            return await _dbContext.SaveChangesAsync();
        }
        private async Task<int> DeleteChangePlanItemsAsync(Guid changePlanId)
        {
            var changePlanItems = await _dbContext.ChangePlanItems
               .Where(x => x.Id == changePlanId)
               .AsNoTracking()
               .ToListAsync();
            foreach(ChangePlanItem changePlanItem in changePlanItems)
            {
                _dbContext.ChangePlanItems.Remove(changePlanItem);
            }
            return await _dbContext.SaveChangesAsync();
        }
    }
}
