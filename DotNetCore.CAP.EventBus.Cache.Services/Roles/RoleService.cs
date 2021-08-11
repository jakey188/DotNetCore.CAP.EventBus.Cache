using DotNetCore.CAP.Cap.EventBus.Cache.Core;
using DotNetCore.CAP.Cap.EventBus.Cache.Core.Caching;
using DotNetCore.CAP.Cap.EventBus.Cache.Data.Entites;
using DotNetCore.CAP.Cap.EventBus.Cache.Services.Roles.Caching;
using CSRedis;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRepository<RoleEntity> _roleRepositoty;
        private readonly CSRedisClient _redis;
        public RoleService(IRepository<RoleEntity> roleRepositoty,
            CSRedisClient redis)
        {
            _roleRepositoty = roleRepositoty;
            _redis = redis;
        }

        public async Task Add(RoleEntity entity)
        {
            await _roleRepositoty.AddAsync(entity, true);
        }

        public async Task Delete(string roleId)
        {
            await _roleRepositoty.DeleteAsync(c => c.Id == roleId, true);
        }

        public async Task<RoleEntity> GetRoleAsync(string roleId)
        {
            var cacheKey = RoleCacheDefaults.GetRoleByIdCacheKey(roleId);

            var role = await _redis.GetAsync(cacheKey, async () =>
            {
                return await _roleRepositoty.Find(c => c.Id == roleId).FirstOrDefaultAsync();
            },
            TimeSpan.FromDays(1));

            return role;
        }

        public async Task<List<RoleEntity>> GetRoleListByAppIdAsync(string appId)
        {
            var cacheKey = RoleCacheDefaults.GetRoleListByAppIdCacheKey(appId);

            var roles = await _redis.GetAsync(cacheKey, async () =>
            {
                return await _roleRepositoty.Find(c => c.AppId == appId).ToListAsync();
            },
            TimeSpan.FromDays(1));

            return roles;
        }
    }
}
