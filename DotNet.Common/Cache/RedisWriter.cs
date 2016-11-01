using DotNet.Common.Cache.Redis;
using NPOI.SS.Formula.Functions;

namespace DotNet.Common.Cache
{
    public class RedisWriter:ICacheWriter
    {

        public void Add(string key, object value, System.DateTime exp)
        {
            using (var client = RedisManager.GetClient())
                client.Set<object>(key, value, exp);
        }

        public void Add(string key, object value)
        {
            using (var client = RedisManager.GetClient())
                client.Set<object>(key, value);
        }

        public object Get(string key)
        {
            using (var client = RedisManager.GetClient())
                return client.Get<T>(key);
        }

        public void Update(string key, object value)
        {
            using (var client = RedisManager.GetClient())
                client.Set(key, value);
        }

        public void Update(string key, object value, System.DateTime exp)
        {
            using (var client = RedisManager.GetClient())
                client.Set(key, value,exp);
        }

        public void Remove(string key)
        {
            using (var client = RedisManager.GetClient())
                client.Remove(key);
        }
    }
}