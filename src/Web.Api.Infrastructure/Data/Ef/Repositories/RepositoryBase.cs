using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Web.Api.Core.Domain.Entities;
using Web.Api.Core.Dto;
using Web.Api.Core.Dto.GatewayResponses;
using Web.Api.Core.Interfaces;
using Web.Api.Core.Interfaces.Gateways.Repositories;
using Web.Api.Infrastructure.Data.Entities;

namespace Web.Api.Infrastructure.Data.Ef.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly ApplicationDbContext Context;

        protected RepositoryBase(ApplicationDbContext context)
        {
            Context = context;
        }

        public IQueryable<T> FindAll()
        {
            return Context
                .Set<T>()
                .AsNoTracking();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return Context
                .Set<T>()
                .Where(expression)
                .AsNoTracking();
        }

        public void Create(T entity)
        {
            Context
                .Set<T>()
                .Add(entity);
        }

        public void Update(T entity)
        {
            Context
                .Set<T>()
                .Update(entity);
        }

        public void Delete(T entity)
        {
            Context
                .Set<T>()
                .Remove(entity);
        }
    }

    //public class UserRepository : IUserRepository
    //{
    //    private UserManager<ApplicationUser> _userManager;
    //    private IMapper _mapper;

    //    public UserRepository(UserManager<ApplicationUser> userManager, IMapper mapper)
    //    {
    //        _userManager = userManager;
    //        _mapper = mapper;
    //    }

    //    public async Task<User> GetUserByIdAsync(string id)
    //    {
    //        return _mapper.Map<User>(await _userManager.FindByIdAsync(id));
    //    }

    //    public async Task<IEnumerable<User>> GetAllUsersAsync(int pageSize)
    //    {
    //        //return await FindAll()
    //        //    .Take(pageSize)
    //        //    .ToListAsync();
    //        throw new NotImplementedException();
    //    }

    //    public async Task<IEnumerable<User>> GetAllUsersAsync()
    //    {
    //        //return await FindAll()
    //        //    .ToListAsync();
    //        throw new NotImplementedException();
    //    }

    //    public Task<GatewayResponseBase> CreateUser(User user)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<GatewayResponseBase> UpdateUser(User user)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public Task<GatewayResponseBase> DeleteUser(User user)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}


}
