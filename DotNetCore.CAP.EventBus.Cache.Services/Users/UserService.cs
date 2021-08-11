using DotNetCore.CAP.Cap.EventBus.Cache.Core;
using DotNetCore.CAP.Cap.EventBus.Cache.Core.Caching;
using DotNetCore.CAP.Cap.EventBus.Cache.Data.Entites;
using DotNetCore.CAP.Cap.EventBus.Cache.Services.Users.Caching;
using CSRedis;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<UserEntity> _userRepositoty;
        private readonly CSRedisClient _redis;
        public UserService(IRepository<UserEntity> userRepositoty,
            CSRedisClient redis)
        {
            _userRepositoty = userRepositoty;
            _redis = redis;
        }

        public async Task Add(UserEntity entity)
        {
            await _userRepositoty.AddAsync(entity,true);
        }

        public async Task Delete(string userId)
        {
            await _userRepositoty.DeleteAsync(c => c.Id == userId, true);
        }

        public async Task<UserEntity> GetUserAsync(string userId)
        {
            var cacheKey = UserCacheDefaults.GetUserInfoByIdCacheKey(userId);

            var user = await _redis.GetAsync(cacheKey, async () =>
            {
                return await _userRepositoty.Find(c => c.Id == userId).FirstOrDefaultAsync();
            }, 
            TimeSpan.FromDays(1));

            return user;
        }
    }
}
