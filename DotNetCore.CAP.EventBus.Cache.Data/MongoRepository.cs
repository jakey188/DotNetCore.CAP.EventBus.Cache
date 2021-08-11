using DotNetCore.CAP.Cap.EventBus.Cache.Core;
using DotNetCore.CAP.Cap.EventBus.Cache.Core.Caching;
using DotNetCore.CAP;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Data
{
    public class MongoRepository<TEntity> : IRepository<TEntity>  where TEntity : BaseEntity
    {
        protected static IMongoDatabase _database;
        private readonly ICapPublisher _cacheEvent;

       
        public MongoRepository(IMongoDatabase database,
            ICapPublisher cacheEvent)
        {
            _database = database;
            _cacheEvent = cacheEvent;
        }

        public IMongoCollection<TEntity> Collection
        {
            get { return _database.GetCollection<TEntity>(typeof(TEntity).Name); }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> AddAsync(TEntity entity, bool publishCacheEvent = false)
        {
            await Collection.InsertOneAsync(entity);

            if (publishCacheEvent)
            {
                await Publish(entity, EntityType.Insert);
            }
            return entity;
        }

        
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<bool> UpdateAsync(Expression<Func<TEntity, bool>> filter, TEntity entity, bool publishCacheEvent = false)
        {
            var update = await Collection.ReplaceOneAsync(filter, entity);

            if (publishCacheEvent)
            {
                await Publish(entity, EntityType.Update);
            }

            return update.ModifiedCount > 0;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> exp, bool publishCacheEvent = false)
        {
            //var delete = await Collection.DeleteOneAsync(exp);
            var entity = await Collection.FindOneAndDeleteAsync(exp);

            if (publishCacheEvent)
            {
                await Publish(entity, EntityType.Delete);
            }

            return entity != null; //delete.DeletedCount > 0;
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> exp)
        {
            var query = Collection.AsQueryable();
            if (exp == null)
                return query;
            return query.Where(exp);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <param name="exp"></param>
        /// <returns></returns>
        public IFindFluent<TEntity, TEntity> Find(Expression<Func<TEntity, bool>> exp)
        {
            return Collection.Find(exp);
        }

        /// <summary>
        /// 发布缓存通知
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private async Task Publish(TEntity entity, EntityType type)
        {
            if (entity == null) await Task.CompletedTask;

            var name = typeof(TEntity).Name;
            var header = new Dictionary<string, string>()
            {
                [CacheDefaults.EntityName] = name,
                [CacheDefaults.EntityType] = type.ToString()
            };
            await _cacheEvent.PublishAsync(CacheDefaults.EventName, entity, header);
        }


    }
}
