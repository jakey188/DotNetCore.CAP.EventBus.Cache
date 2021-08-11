using CSRedis;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Core.Caching
{
    public static class CSRedisClientExtension
    {
        /// <summary>
        /// 设置or读取缓存
        /// </summary>
        /// <typeparam name="T">当前对象</typeparam>
        /// <param name="redis">ICache</param>
        /// <param name="key">缓存key</param>
        /// <param name="acquire">结果集</param>
        /// <param name="cacheTime">缓存绝对时间</param>
        /// <remarks>如果结果集为null,直接return null,只有结果集为null时不会写入缓存</remarks>
        /// <returns></returns>
        public static async Task<T> GetAsync<T>(this CSRedisClient redis, string key, Func<Task<T>> acquire, TimeSpan? cacheTime)
        {
            if (await redis.ExistsAsync(key))
            {
                var value = await redis.GetAsync(key);
                return JSON.DeserializeObject<T>(value);
            }
            var result = await acquire();
            if (result != null)
            {
                await redis.SetAsync(key, JSON.SerializeObject(result), cacheTime.HasValue ? (int)cacheTime.Value.TotalSeconds : -1);
            }
            return result;
        } 
    }
}
