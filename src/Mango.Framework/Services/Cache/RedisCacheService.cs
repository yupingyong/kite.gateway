using System;
using System.Text;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
namespace Mango.Framework.Services.Cache
{
    public class RedisCacheService:ICacheService
    {
        private RedisCache _redisCache = null;
        public RedisCacheService(RedisCacheOptions options)
        {
            _redisCache = new RedisCache(options);
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">泛型(返回的结果类型)</typeparam>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        public T Get<T>(string key) where T:new()
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(_redisCache.Get(key)));
                }
                return default(T);
            }
            catch
            {
                return default(T);
            }
        }
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        public string Get(string key)
        {
            try
            {
                if (!string.IsNullOrEmpty(key))
                {
                    return Encoding.UTF8.GetString(_redisCache.Get(key));
                }
                return string.Empty;
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值</param>
        /// <param name="ExpirationTime">绝对过期时间(分钟)</param>
        public void Add(string key,object value,int expirationTime=20)
        {
            if (!string.IsNullOrEmpty(key))
            {
                _redisCache.Set(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(expirationTime)
                });
            }
        }
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        public void Remove(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                _redisCache.Remove(key);
            }
        }
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值</param>
        /// <param name="ExpirationTime"></param>
        public void Replace(string key, object value, int expirationTime = 20)
        {
            if (!string.IsNullOrEmpty(key))
            {
                _redisCache.Remove(key);
                _redisCache.Set(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)), new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(expirationTime)
                });
            }
        }
    }
}

