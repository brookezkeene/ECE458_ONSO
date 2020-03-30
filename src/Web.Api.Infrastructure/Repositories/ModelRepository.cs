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
using Web.Api.Infrastructure.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Infrastructure.Repositories
{
    public class ModelRepository<TDbContext> : IModelRepository
        where TDbContext : DbContext, IApplicationDbContext
    {
        private readonly TDbContext _dbContext;

        public ModelRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        

        public async Task<PagedList<Model>> GetModelsAsync(string vendor, string number, int heightStart, int heightEnd,
                int networkRangeStart, int networkRangeEnd, int powerRangeStart, int powerRangeEnd,
                int memoryRangeStart, int memoryRangeEnd, string sortBy, string isDesc, int page = 1, int pageSize = 10)
        {
            var pagedList = new PagedList<Model>();
            Expression<Func<Model, bool>> vendorCondition = x => (x.Vendor.Contains(vendor));
            Expression<Func<Model, bool>> numberCondition = x => (x.ModelNumber.Contains(number));
            var models = await Sort( vendor,  number,  heightStart,  heightEnd,
                 networkRangeStart,  networkRangeEnd,  powerRangeStart,  powerRangeEnd,
                 memoryRangeStart,  memoryRangeEnd,  sortBy,  isDesc,  page = 1,  pageSize = 10);
            pagedList.AddRange(models);
            pagedList.TotalCount = await _dbContext.Models
                .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                .Where(x => x.Height >= heightStart && x.Height <= heightEnd &&
                                x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                                x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd &&
                                x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
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
                //.Include(x => x.NetworkPorts)
                //.AsNoTracking()
                .ToListAsync();
            //models = models.Where(x => x.ModelNumber.Contains(search) || x.Vendor.Contains(search)).ToList();
            return models;

        }
        public async Task<Model> GetModelAsync(Guid modelId)
        {
            return await _dbContext.Models
                //.Include(x => x.Assets)
                //.Include(x => x.NetworkPorts)
                .Where(x => x.Id == modelId)
                //.AsNoTracking()
                .SingleAsync();
        }

        public async Task<Model> FindModel(Expression<Func<Model, bool>> expr)
        {
            return await _dbContext.Models
                .Where(expr)
                //.AsNoTracking()
                .SingleOrDefaultAsync();
        }

        public async Task<int> AddModelAsync(Model model)
        {
            NetworkPortsSameNumberAsEthernetPorts(model);
            _dbContext.Models.Add(model);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<int> UpdateModelAsync(Model model)
        {
            NetworkPortsSameNumberAsEthernetPorts(model);
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

        public Model GetModel(string vendor, string modelNumber)
        {
            return _dbContext.Models
                //.Include(model => model.NetworkPorts)
                //.AsNoTracking()
                .SingleOrDefault(model => model.Vendor == vendor && model.ModelNumber == modelNumber);
        }

        public Model GetModel(Guid modelId)
        {
            return _dbContext.Models
                //.AsNoTracking()
                .SingleOrDefault(model => model.Id == modelId);
        }

        private static void NetworkPortsSameNumberAsEthernetPorts(Model model)
        {
            // 1. sort by port number ascending
            var ports = model.NetworkPorts.OrderBy(o => o.Number).ToList();

            // 2. remove all ports in excess of # dictated by property
            ports = ports.Take(model.EthernetPorts.GetValueOrDefault()).ToList();

            // 3. add ports as necessary
            var missingPorts = Enumerable.Range(1, model.EthernetPorts.GetValueOrDefault())
                .Where(n => model.NetworkPorts.SingleOrDefault(o => o.Number == n) == null)
                .Select(n => new ModelNetworkPort {Number = n, Name = n.ToString()});
            ports.AddRange(missingPorts);

            // 4. default empty port names to the port number
            foreach (var port in ports.Where(port => string.IsNullOrWhiteSpace(port.Name)))
            {
                port.Name = port.Number.ToString();
            }

            // 5. done
            model.NetworkPorts = ports;
        }
        private async Task<List<Model>> Sort(string vendor, string number, int heightStart, int heightEnd,
                int networkRangeStart, int networkRangeEnd, int powerRangeStart, int powerRangeEnd,
                int memoryRangeStart, int memoryRangeEnd, string sortBy, string isDesc, int page = 1, int pageSize = 10)
        {
            Expression<Func<Model, bool>> vendorCondition = x => (x.Vendor.Contains(vendor));
            Expression<Func<Model, bool>> numberCondition = x => (x.ModelNumber.Contains(number));

            if (string.IsNullOrEmpty(sortBy))
            {
                return await _dbContext.Models
                    .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                    .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                    .Where(x => x.Height >= heightStart && x.Height <= heightEnd && x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                            x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd && x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
                    .PageBy(x => x.Id, page, pageSize)
                    .ToListAsync();
            }
            else if (sortBy.Equals("vendor"))
            {
                if (isDesc.Equals("false"))
                {
                    return await _dbContext.Models
                    .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                    .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                    .Where(x => x.Height >= heightStart && x.Height <= heightEnd && x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                            x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd && x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
                    .PageBy(x => x.Vendor, page, pageSize, false)
                    .ToListAsync();
                }
                else if (isDesc.Equals("true"))
                {
                    return await _dbContext.Models
                    .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                    .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                    .Where(x => x.Height >= heightStart && x.Height <= heightEnd && x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                            x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd && x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
                    .PageBy(x => x.Vendor, page, pageSize, true)
                    .ToListAsync();
                }
            }
            else if (sortBy.Equals("modelNumber"))
            {
                if (isDesc.Equals("false"))
                {
                    return await _dbContext.Models
                        .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                        .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                        .Where(x => x.Height >= heightStart && x.Height <= heightEnd && x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                            x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd && x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
                        .PageBy(x => x.ModelNumber, page, pageSize, false)
                        .ToListAsync();
                }
                else if (isDesc.Equals("true"))
                {
                    return await _dbContext.Models
                        .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                        .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                       .Where(x => x.Height >= heightStart && x.Height <= heightEnd && x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                            x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd && x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
                        .PageBy(x => x.ModelNumber, page, pageSize, true)
                        .ToListAsync();
                }
            }
            else if (sortBy.Equals("height"))
            {
                if (isDesc.Equals("false"))
                {
                    return await _dbContext.Models
                        .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                        .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                        .Where(x => x.Height >= heightStart && x.Height <= heightEnd && x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                            x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd && x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
                        .PageBy(x => x.Height, page, pageSize, false)
                        .ToListAsync();
                }
                else if (isDesc.Equals("true"))
                {
                    return await _dbContext.Models
                        .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                        .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                        .Where(x => x.Height >= heightStart && x.Height <= heightEnd && x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                            x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd && x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
                        .PageBy(x => x.Height, page, pageSize, true)
                        .ToListAsync();
                }
            }
            else if (sortBy.Equals("ethernetPorts"))
            {
                if (isDesc.Equals("false"))
                {
                    return await _dbContext.Models
                        .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                        .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                        .Where(x => x.Height >= heightStart && x.Height <= heightEnd && x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                            x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd && x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
                        .PageBy(x => x.EthernetPorts, page, pageSize, false)
                        .ToListAsync();
                }
                else if (isDesc.Equals("true"))
                {
                    return await _dbContext.Models
                        .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                        .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                        .Where(x => x.Height >= heightStart && x.Height <= heightEnd && x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                            x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd && x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
                        .PageBy(x => x.EthernetPorts, page, pageSize, true)
                        .ToListAsync();
                }
            }
            else if (sortBy.Equals("powerPorts"))
            {
                if (isDesc.Equals("false"))
                {
                    return await _dbContext.Models
                        .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                        .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                        .Where(x => x.Height >= heightStart && x.Height <= heightEnd && x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                            x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd && x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
                        .PageBy(x => x.PowerPorts, page, pageSize, false)
                        .ToListAsync();
                }
                else if (isDesc.Equals("true"))
                {
                    return await _dbContext.Models
                         .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                         .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                         .Where(x => x.Height >= heightStart && x.Height <= heightEnd && x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                            x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd && x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
                         .PageBy(x => x.PowerPorts, page, pageSize, true)
                         .ToListAsync();
                }
            }
            else if (sortBy.Equals("cpu"))
            {
                if (isDesc.Equals("false"))
                {
                    return await _dbContext.Models
                         .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                         .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                         .Where(x => x.Height >= heightStart && x.Height <= heightEnd && x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                            x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd && x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
                         .PageBy(x => x.Cpu, page, pageSize, false)
                         .ToListAsync();
                }
                else if (isDesc.Equals("true"))
                {
                    return await _dbContext.Models
                        .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                        .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                        .Where(x => x.Height >= heightStart && x.Height <= heightEnd && x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                            x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd && x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
                        .PageBy(x => x.Cpu, page, pageSize, true)
                        .ToListAsync();
                }
            }
            else //if (sortBy.Equals("memory"))
            {
                if (isDesc.Equals("false"))
                {
                    return await _dbContext.Models
                         .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                         .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                         .Where(x => x.Height >= heightStart && x.Height <= heightEnd && x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                            x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd && x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
                         .PageBy(x => x.Memory, page, pageSize, false)
                         .ToListAsync();
                }
                else //if (isDesc.Equals("true"))
                {
                    return await _dbContext.Models
                        .WhereIf(!string.IsNullOrEmpty(vendor), vendorCondition)
                        .WhereIf(!string.IsNullOrEmpty(number), numberCondition)
                        .Where(x => x.Height >= heightStart && x.Height <= heightEnd && x.EthernetPorts >= networkRangeStart && x.EthernetPorts <= networkRangeEnd &&
                            x.PowerPorts >= powerRangeStart && x.PowerPorts <= powerRangeEnd && x.Memory >= memoryRangeStart && x.Memory <= memoryRangeEnd)
                        .PageBy(x => x.Memory, page, pageSize, true)
                        .ToListAsync();
                }
            }
            return null;
        }
    }
}
