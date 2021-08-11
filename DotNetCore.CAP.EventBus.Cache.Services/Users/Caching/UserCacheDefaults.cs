using DotNetCore.CAP.Cap.EventBus.Cache.Core;
using DotNetCore.CAP.Cap.EventBus.Cache.Core.Caching;
using DotNetCore.CAP.Cap.EventBus.Cache.Data.Entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Services.Users.Caching
{
    public static class UserCacheDefaults
    {
        public static string GetUserInfoByIdCacheKey(string id)
        {
            return $"{GetPrefixCacheKey}:{nameof(GetUserInfoByIdCacheKey)}:" + id;
        }

        public static string GetPrefixCacheKey => CacheDefaults.EntityCachePrefix + typeof(UserEntity).Name;
        
    }
}
