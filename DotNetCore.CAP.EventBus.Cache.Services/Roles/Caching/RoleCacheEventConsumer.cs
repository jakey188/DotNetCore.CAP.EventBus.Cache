using DotNetCore.CAP.Cap.EventBus.Cache.Core;
using DotNetCore.CAP.Cap.EventBus.Cache.Core.Caching;
using DotNetCore.CAP.Cap.EventBus.Cache.Data.Entites;
using DotNetCore.CAP.Cap.EventBus.Cache.Services.Consumers;
using DotNetCore.CAP.Cap.EventBus.Cache.Services.Roles.Caching;
using CSRedis;
using DotNetCore.CAP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Services.Caches
{
    public class RoleCacheEventConsumer : IConsumer
    {
        private readonly CSRedisClient _redis;
        public RoleCacheEventConsumer(CSRedisClient redis)
        {
            _redis = redis;
        }

        public string EntityName => typeof(RoleEntity).Name;

        public async Task ClearCache(CacheEntity obj)
        {
            var entity = JsonConvert.DeserializeObject<RoleEntity>(obj.Entity);

            if (obj.EntityType == EntityType.Insert)
            {
                await _redis.DelAsync(RoleCacheDefaults.GetRoleListByAppIdCacheKey(entity.AppId));
            }
            else
            {
                await _redis.DelAsync(RoleCacheDefaults.GetRoleListByAppIdCacheKey(entity.AppId),
                    RoleCacheDefaults.GetRoleByIdCacheKey(entity.Id));
            }

            await Task.CompletedTask;
        }
    }
}
