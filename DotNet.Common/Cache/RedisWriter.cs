namespace DotNet.Common.Cache
{
    public class RedisWriter:ICacheWriter
    {

        public void Add(string key, object value, System.DateTime exp)
        {
            throw new System.NotImplementedException();
        }

        public void Add(string key, object value)
        {
            throw new System.NotImplementedException();
        }

        public object Get(string key)
        {
            throw new System.NotImplementedException();
        }

        public void Update(string key, object value)
        {
            throw new System.NotImplementedException();
        }

        public void Update(string key, object value, System.DateTime exp)
        {
            throw new System.NotImplementedException();
        }

        public void Remove(string key)
        {
            throw new System.NotImplementedException();
        }
    }
}