using DotNetCore.CAP.Cap.EventBus.Cache.Core.Caching;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Core
{
    public interface IConsumer
    {
        string EntityName { get; }
        Task ClearCache(CacheEntity entity);
    }
}
