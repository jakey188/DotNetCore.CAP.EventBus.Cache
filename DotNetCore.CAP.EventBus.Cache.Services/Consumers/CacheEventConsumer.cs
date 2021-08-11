using DotNetCore.CAP.Cap.EventBus.Cache.Core;
using DotNetCore.CAP.Cap.EventBus.Cache.Core.Caching;
using DotNetCore.CAP;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Services.Consumers
{
    public class CacheEventConsumer : ICapSubscribe
    {
        private readonly IEnumerable<IConsumer> _consumers;
        public CacheEventConsumer(IEnumerable<IConsumer> consumers)
        {
            _consumers = consumers;
        }

        /// <summary>
        /// 订阅缓存通知
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [CapSubscribe(CacheDefaults.EventName)]
        public async Task Invoke(object entity, [FromCap] CapHeader header)
        {
            var name = header[CacheDefaults.EntityName];
            var type = header[CacheDefaults.EntityType];
            var consumber = _consumers.FirstOrDefault(c => c.EntityName == name);
            if (consumber != null)
            {
                if (Enum.TryParse(type, out EntityType entityType))
                {
                    await consumber.ClearCache(new CacheEntity(name, entityType, JsonConvert.SerializeObject(entity)));
                }
            }
        }
    }
}
