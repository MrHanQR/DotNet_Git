using System;
using System.Configuration;
using Memcached.ClientLibrary;

namespace DotNet.Common.Cache
{
    public class MemcachedWriter : ICacheWriter
    {
        public static readonly MemcachedClient MemcachedClient;

        static MemcachedWriter()
        {
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["MemcachedServer"]))
            {
                throw new Exception("Memcached节点配置异常，请联系管理员");
            }
            else
            {
                string[] servers = ConfigurationManager.AppSettings["MemcachedServer"].Split(',');
                SockIOPool pool = SockIOPool.GetInstance();
                pool.SetServers(servers);
                pool.InitConnections = 3;
                pool.MinConnections = 3;
                pool.MaxConnections = 5;
                pool.SocketConnectTimeout = 1000;
                pool.SocketTimeout = 3000;
                pool.MaintenanceSleep = 30;
                pool.Failover = true;
                pool.Nagle = false;
                pool.Initialize();
                MemcachedClient = new MemcachedClient();
                MemcachedClient.EnableCompression = false;
            }
        }
        public void Add(string key, object value, System.DateTime exp)
        {
            MemcachedClient.Add(key, value, exp);
        }

        public void Add(string key, object value)
        {
            MemcachedClient.Add(key, value, DateTime.MinValue);
        }

        public object Get(string key)
        {
            return MemcachedClient.Get(key);
        }
        public void Update(string key, object value)
        {
            MemcachedClient.Set(key, value);
        }
        public void Update(string key, object value, DateTime exp)
        {
            MemcachedClient.Set(key, value, exp);
        }
        public void Remove(string key)
        {
            MemcachedClient.Delete(key);
        }   
    }
}