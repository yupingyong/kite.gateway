using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Framework.Services.Cache
{
    public interface ICacheService
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">泛型(返回的结果类型)</typeparam>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        T Get<T>(string key) where T : new();
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        string Get(string key);
        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="value">缓存值</param>
        /// <param name="ExpirationTime">绝对过期时间(分钟)</param>
        void Add(string key, object value, int expirationTime = 20);
        /// <summary>
        /// 移除缓存
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);
        /// <summary>
        /// 更新缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="ExpirationTime"></param>
        void Replace(string key, object value, int expirationTime = 20);
    }
}
