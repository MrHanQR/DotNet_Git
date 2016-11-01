using System;
using DotNet.Common.Cache;
using DotNet.Entity.Enum;

namespace DotNet.Common
{
    public class CacheHelper
    {
        private static ICacheWriter _cacheWriter = null;
        private static readonly object lockObj = new object();
        public static ICacheWriter CacheWriter
        {
            get
            {
                if (_cacheWriter == null)
                {
                    lock (lockObj)
                    {
                        switch (ConfigHelper.CacheType)
                        {
                            case CacheTypeEnum.HttpRuntime:
                                _cacheWriter = new HttpRuntimeCachedWriter();
                                break;
                            case CacheTypeEnum.Memcached:
                                _cacheWriter = new MemcachedWriter();
                                break;
                            case CacheTypeEnum.Redis:
                                _cacheWriter = new RedisWriter();
                                break;
                            default:
                                _cacheWriter = new HttpRuntimeCachedWriter();
                                break;
                        }
                    }
                }
                return _cacheWriter;
            }
        }

        public static void Add(string key, object value, DateTime exp)
        {
            CacheWriter.Add(key, value, exp);
        }
        public static void Add(string key, object value)
        {
            CacheWriter.Add(key, value);
        }

        public static object Get(string key)
        {
            return CacheWriter.Get(key);
        }

        public static void Update(string key, object value)
        {
            CacheWriter.Update(key, value);
        }

        public static void Remove(string key)
        {
            CacheWriter.Remove(key);
        }
    }
}