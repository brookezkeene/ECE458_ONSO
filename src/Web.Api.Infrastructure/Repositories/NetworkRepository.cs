using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Web.Api.Infrastructure.Entities;
using Web.Api.Infrastructure.Extensions;
using Web.Api.Infrastructure.Interfaces;
using Web.Api.Infrastructure.Repositories.Interfaces;

namespace Web.Api.Infrastructure.Repositories
{
    public class NetworkRepository<TDbContext> : INetworkRepository
        where TDbContext : DbContext, IApplicationDbContext
    {
        private readonly TDbContext _dbContext;

        public NetworkRepository(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> AddConnectionAsync(NetworkConnection connection)
        {
            await _dbContext.NetworkConnections.AddAsync(connection);
            await _dbContext.SaveChangesAsync();

            _dbContext.DeletePreviousNetworkConnections();

            return connection.Id;
        }

        public AssetNetworkPort GetNetworkPort(Guid portId)
        {
            return _dbContext.AssetNetworkPort.Find(portId);
        }

        public AssetNetworkPort GetNetworkPort(string hostname, string portName)
        {
            return _dbContext.AssetNetworkPort
                .SingleOrDefault(port => port.Asset.Hostname == hostname && port.ModelNetworkPort.Name == portName);
        }

        public async Task<NetworkConnection> GetConnectionAsync(Guid connectionId)
        {
            return await _dbContext.NetworkConnections.FindAsync(connectionId);
        }

        public async Task<List<Guid>> AddConnectionsAsync(List<NetworkConnection> connections)
        {
            await _dbContext.NetworkConnections.AddRangeAsync(connections);
            await _dbContext.SaveChangesAsync();

            _dbContext.DeletePreviousNetworkConnections();


            return connections.Select(conn => conn.Id)
                .ToList();
        }
    }
}