using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Core
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> AddAsync(TEntity entity, bool publishCacheEvent = false);
        Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> filter, bool publishCacheEvent = false);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> filter);
        IFindFluent<TEntity, TEntity> Find(Expression<Func<TEntity, bool>> filter);
        Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> exp, TEntity entity, bool publishCacheEvent = false);
    }
}
