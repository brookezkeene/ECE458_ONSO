using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Web.Api.Common;
using Web.Api.Common.Extensions;
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
        public async Task<PagedList<ChangePlan>> GetChangePlansAsync(Guid? createdById, int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<ChangePlan>();

            var changePlans = await _dbContext.ChangePlans
                .Where(x => x.CreatedById == createdById)
                .PageBy(x => x.Id, page, pageSize)
                .AsNoTracking()
                .ToListAsync();
            pagedList.AddRange(changePlans);
            pagedList.TotalCount = await _dbContext.ChangePlans
                .Where(x => x.Id == createdById)
                .CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.CurrentPage = page;

            return pagedList;
        }
        public async Task<List<ChangePlanItem>> GetChangePlanItemsAsync(Guid changePlanId)
        {
            var changePlans = await _dbContext.ChangePlanItems
                .Where(x => x.ChangePlanId == changePlanId)
                .AsNoTracking()
                .ToListAsync();
            var list = changePlans.OrderBy(q => q.CreatedDate).ToList();
            return list;
        }
        public async Task<int> AddChangePlanAsync(ChangePlan changePlan)
        {
            _dbContext.ChangePlans.Add(changePlan);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> AddChangePlanItemAsync(ChangePlanItem changePlanItem)
        {
            
            if (!await _dbContext.ChangePlanItems.AnyAsync(o => o.AssetId == changePlanItem.AssetId && 
                    o.ChangePlanId == changePlanItem.ChangePlanId && changePlanItem.ExecutionType.Equals("update")))
            {
                _dbContext.ChangePlanItems.Add(changePlanItem);
                return await _dbContext.SaveChangesAsync();
            }
            var updatedChangePlan = await _dbContext.ChangePlanItems
                .Where(x => x.AssetId == changePlanItem.AssetId && x.ChangePlanId == changePlanItem.ChangePlanId)
                .AsNoTracking()
                .SingleOrDefaultAsync();
            updatedChangePlan.NewData = changePlanItem.NewData;

            return await UpdateChangePlanItemAsync(updatedChangePlan);
        }
        public async Task<int> UpdateChangePlanAsync(ChangePlan changePlan)
        {
            _dbContext.ChangePlans.Update(changePlan);
            return await _dbContext.SaveChangesAsync();
        }
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
            foreach (ChangePlanItem changePlanItem in changePlanItems)
            {
                _dbContext.ChangePlanItems.Remove(changePlanItem);
            }
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> ExecuteChangePlan(List<ChangePlanItem> changePlanItems)
        {

            for (int i = 0; i < changePlanItems.Count(); i ++ ) 
            {   
                var changePlanItem = changePlanItems[i];
                if (changePlanItem.ExecutionType.Equals("create"))
                {
                    var asset = JsonConvert.DeserializeObject<Asset>(changePlanItem.NewData);
                    //need to set these equal to null otherwise error occurs in database 
                    //these values weren't null previously so that feilds which use them could appear on the 
                    //data table for the change plan
                    asset.Model = null;
                    asset.Rack = null;
                    asset.Owner = null;
                    _dbContext.Assets.Add(asset);
                }
                else if (changePlanItem.ExecutionType.Equals("update"))
                {
                    var asset = JsonConvert.DeserializeObject<Asset>(changePlanItem.NewData);
                    _dbContext.Assets.Update(asset);
                }
                else if (changePlanItem.ExecutionType.Equals("decommission"))
                {
                    var decommissionedAsset = JsonConvert.DeserializeObject<DecommissionedAsset>(changePlanItem.NewData);
                    var asset = await _dbContext.Assets
                                .Where(x => x.Id == decommissionedAsset.Id)
                                .AsNoTracking()
                                .SingleOrDefaultAsync();
                    _dbContext.DecommissionedAssets.Add(decommissionedAsset);
                    _dbContext.Assets.Remove(asset);

                }
            }
            return await _dbContext.SaveChangesAsync();
        }

    }
}
