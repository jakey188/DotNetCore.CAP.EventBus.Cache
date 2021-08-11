using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Core
{
    public abstract class BaseEntity
    {
        public string Id { get; set; }
        public string AppId { get; set; }
    }
}
