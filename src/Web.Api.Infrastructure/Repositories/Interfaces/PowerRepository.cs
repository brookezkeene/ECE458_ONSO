using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Extensions;
using Web.Api.Infrastructure.Interfaces;

namespace Web.Api.Infrastructure.Repositories.Interfaces
{
    public class PowerRepository<TDbContext> : IPowerRepository
        where TDbContext : DbContext, IApplicationDbContext
    {
        private readonly TDbContext _dbContext;

        public PowerRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> CreateConnectionAsync(PowerConnection connection)
        {
            await _dbContext.PowerConnections.AddAsync(connection);
            await _dbContext.SaveChangesAsync();

            _dbContext.DeletePreviousPowerConnections();
            

            return connection.Id;
        }

        public async Task<PowerConnection> GetConnectionAsync(Guid connectionId)
        {
            return await _dbContext.PowerConnections.FindAsync(connectionId);
        }

        public AssetPowerPort GetAssetPowerPort(Guid assetPowerPortId)
        {
            return _dbContext.AssetPowerPort.Find(assetPowerPortId);
        }

        public PduPort GetPduPort(Guid pduPortId)
        {
            return _dbContext.PduPort.Find(pduPortId);
        }

        public PduPort GetPduPort(Guid rackId, PduLocation pduLocation, int portNumber)
        {
            return _dbContext.PduPort
                .SingleOrDefault(port => port.Pdu.RackId == rackId && port.Pdu.Location == pduLocation && port.Number == portNumber);
        }

        public List<AssetPowerPort> GetAssetPowerPorts(Guid assetId)
        {
            return _dbContext.AssetPowerPort
                .Where(port => port.AssetId == assetId)
                .ToList();
        }

        public async Task<List<Guid>> CreateConnectionsAsync(List<PowerConnection> connections)
        {
            await _dbContext.PowerConnections.AddRangeAsync(connections);
            await _dbContext.SaveChangesAsync();

            _dbContext.DeletePreviousPowerConnections();
            

            return connections.Select(conn => conn.Id)
                .ToList();
        }
    }
}