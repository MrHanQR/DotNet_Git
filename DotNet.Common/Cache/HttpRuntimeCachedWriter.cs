using System;
using System.Web;

namespace DotNet.Common.Cache
{
    public class HttpRuntimeCachedWriter : ICacheWriter
    {
        public void Add(string key, object value, System.DateTime exp)
        {
            if (HttpRuntime.Cache.Get(key) != null)
            {
                HttpRuntime.Cache.Remove(key);
            }
            HttpRuntime.Cache.Insert(key, value, null, exp, TimeSpan.Zero);
        }

        public void Add(string key, object value)
        {
            if (HttpRuntime.Cache.Get(key) != null)
            {
                HttpRuntime.Cache.Remove(key);
            }
            HttpRuntime.Cache.Insert(key, value,null,DateTime.Now.AddMinutes(20),TimeSpan.Zero);
        }

        public object Get(string key)
        {
            return HttpRuntime.Cache[key];
        }


        public void Update(string key, object value)
        {
            if (HttpRuntime.Cache.Get(key) != null)
            {
                HttpRuntime.Cache.Remove(key);
            }
            HttpRuntime.Cache.Insert(key, value, null, DateTime.MinValue, TimeSpan.Zero);
        }
        public void Update(string key, object value, DateTime exp)
        {
            if (HttpRuntime.Cache.Get(key) != null)
            {
                HttpRuntime.Cache.Remove(key);
            }
            HttpRuntime.Cache.Insert(key, value, null, exp, TimeSpan.Zero);
        }
        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }
    }
}