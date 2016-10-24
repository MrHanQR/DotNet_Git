using System;

namespace DotNet.Common.Cache
{
    public interface ICacheWriter
        {
            void Add(string key, object value, DateTime exp);
            void Add(string key, object value);
            object Get(string key);
            void Update(string key, object value);
            void Update(string key, object value, DateTime exp);
            void Remove(string key);
        } 
}