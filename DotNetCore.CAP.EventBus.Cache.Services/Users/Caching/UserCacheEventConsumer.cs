using DotNetCore.CAP.Cap.EventBus.Cache.Core;
using DotNetCore.CAP.Cap.EventBus.Cache.Core.Caching;
using DotNetCore.CAP.Cap.EventBus.Cache.Data.Entites;
using DotNetCore.CAP.Cap.EventBus.Cache.Services.Users.Caching;
using CSRedis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Services.Users.Caches
{
    public class UserCacheEventConsumer : IConsumer
    {
        private readonly CSRedisClient _redis;
        public UserCacheEventConsumer(CSRedisClient redis)
        {
            _redis = redis;
        }

        public string EntityName => typeof(UserEntity).Name;

        public async Task ClearCache(CacheEntity obj)
        {
            var entity = JsonConvert.DeserializeObject<UserEntity>(obj.Entity);

            var cacheKey = UserCacheDefaults.GetUserInfoByIdCacheKey(entity.Id);

            if (obj.EntityType != EntityType.Insert)
                await _redis.DelAsync(cacheKey);
        }
    }
}
