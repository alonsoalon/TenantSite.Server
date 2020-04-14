using AlonsoAdmin.Common.Extensions;
using AlonsoAdmin.Common.Utils;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AlonsoAdmin.Common.Cache
{
    public class MemoryCache : ICache
    {

        private readonly IMemoryCache _memoryCache;
        public MemoryCache(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        private List<string> GetAllKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = _memoryCache.GetType().GetField("_entries", flags).GetValue(_memoryCache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }

        #region Remove
        public long Remove(params string[] key)
        {
            foreach (var k in key)
            {
                _memoryCache.Remove(k);
            }
            return key.Length;
        }

        public Task<long> RemoveAsync(params string[] key)
        {
            foreach (var k in key)
            {
                _memoryCache.Remove(k);
            }

            return Task.FromResult(key.Length.ToLong());
        }
        

        public async Task<long> RemoveByPatternAsync(string pattern)
        {
            if (pattern.IsNull())
                return default;

            pattern = Regex.Replace(pattern, @"\{.*\}", "(.*)");

            var keys = GetAllKeys().Where(k => Regex.IsMatch(k, pattern));

            if (keys != null && keys.Count() > 0)
            {
                return await RemoveAsync(keys.ToArray());
            }

            return default;
        }
        #endregion

        #region TryGet
        public bool Exists(string key)
        {
            return _memoryCache.TryGetValue(key, out _);
        }

        public Task<bool> ExistsAsync(string key)
        {
            return Task.FromResult(_memoryCache.TryGetValue(key, out _));
        }
        #endregion

        #region Get
        public string Get(string key)
        {
            return _memoryCache.Get(key)?.ToString();
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }
        public Task<string> GetAsync(string key)
        {
            return Task.FromResult(Get(key));
        }

        public Task<T> GetAsync<T>(string key)
        {
            return Task.FromResult(Get<T>(key));
        }
        #endregion

        #region Set
        public bool Set(string key, object value)
        {
             _memoryCache.Set(key, value);
            return true;

        }
        public bool Set(string key, object value, TimeSpan expire)
        {
            _memoryCache.Set(key, value, expire);
            return true;
        }

        public Task<bool> SetAsync(string key, object value)
        {
            Set(key, value);
            return Task.FromResult(true);
        }

        public Task<bool> SetAsync(string key, object value, TimeSpan expire)
        {
           Set(key, value, expire);
           return Task.FromResult(true);
        }

        #endregion


    }
}
