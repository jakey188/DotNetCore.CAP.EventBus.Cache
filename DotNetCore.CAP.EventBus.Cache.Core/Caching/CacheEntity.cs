using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Core.Caching
{

    public class CacheEntity : CacheEntity<string>
    {
        public CacheEntity(string tableName, EntityType entityType,string entity)
        {
            EntityName = tableName;
            EntityType = entityType;
            Entity = entity;
        }
    }

    public class CacheEntity<T> where T : class
    {
        public string EntityName { get; set; }
        public EntityType EntityType { get; set; }
        public T Entity { get; set; }
    }

    public enum EntityType
    {
        Insert,
        Update,
        Delete
    }
}
