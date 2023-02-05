using Kite.Gateway.Application.Contracts.Dtos.Administrator;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
namespace Kite.Gateway.Admin.Core
{
    public class AuthorizationServerStorage
    {
        private readonly IMemoryCache _memoryCache;
        public AuthorizationServerStorage(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// 判断是否已经登录
        /// </summary>
        /// <returns></returns>
        public bool IsLogin()
        {
            if (_memoryCache.TryGetValue("administrator", out AdministratorDto administrator))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 删除登录会话信息
        /// </summary>
        /// <returns></returns>
        public void DeleteServerStorage()
        {
            if (_memoryCache.TryGetValue("administrator", out AdministratorDto administrator))
            {
                _memoryCache.Remove("administrator");
            }
        }
        /// <summary>
        /// 设置登录会话信息
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetServerStorage(AdministratorDto value)
        {
            if (_memoryCache.TryGetValue("administrator", out AdministratorDto administrator))
            {
                _memoryCache.Remove("administrator");
            }
            _memoryCache.Set("administrator", value, DateTimeOffset.Now.AddMinutes(120));
        }
        /// <summary>
        /// 获取登录会话
        /// </summary>
        /// <returns></returns>
        public AdministratorDto GetServerStorage()
        {
            if (_memoryCache.TryGetValue("administrator", out AdministratorDto administrator))
            {
                return administrator;
            }
            else
            {
                return new AdministratorDto();
            }
        }
    }
}
