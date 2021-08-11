using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetCore.CAP.Cap.EventBus.Cache.Core
{
    public class JSON
    {
        private static JsonSerializerSettings _jsonSerializer = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            NullValueHandling = NullValueHandling.Ignore,
        };

        public static string SerializeObject(object value)
        {
            string result = value is string ? value.ToString() : JsonConvert.SerializeObject(value, _jsonSerializer);
            return result;
        }

        public static T DeserializeObject<T>(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                var t = default(T);
                return t;
            }
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            return JsonConvert.DeserializeObject<T>(value, _jsonSerializer);
        }
    }
}
