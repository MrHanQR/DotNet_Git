using System;
using DotNet.Common.Cache;
using Spring.Context;
using Spring.Context.Support;
using Spring.Objects.Factory;

namespace DotNet.Common
{
    public class CacheHelper
    {
        public static ICacheWriter CacheWriter { get; set; }
        static CacheHelper()
        {
            //静态属性static的话，如果想让他有注入的值必须先创建一个实例，才能注入成功
            //静态方法调用的时候，不需要spring容器创建实例，所以属性无法注入进去，
            //所以在静态构造函数中用spring创建一个类的实例对象，顺便给当前静态的属性做了注入
            IApplicationContext ctx = ContextRegistry.GetContext();
            var obj = ((IObjectFactory)ctx).GetObject("CacheHelper") as ICacheWriter;
            
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