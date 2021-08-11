using DotNetCore.CAP.Cap.EventBus.Cache.Core.Caching;
using DotNetCore.CAP.Cap.EventBus.Cache.Data.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Services.Roles.Caching
{
    public class RoleCacheDefaults
    {
        public static string GetRoleByIdCacheKey(string id)
        {
            return $"{GetPrefixCacheKey}:{nameof(GetRoleByIdCacheKey)}:" + id;
        }

        public static string GetRoleListByAppIdCacheKey(string appId)
        {
            return $"{GetPrefixCacheKey}:{nameof(GetRoleListByAppIdCacheKey)}:" + appId;
        }


        public static string GetPrefixCacheKey => CacheDefaults.EntityCachePrefix + typeof(RoleEntity).Name;
    }
}
