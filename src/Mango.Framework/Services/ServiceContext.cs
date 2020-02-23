using System;
using System.Collections.Generic;
using System.Text;
using Mango.Framework.Services.Cache;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
namespace Mango.Framework.Services
{
    public class ServiceContext
    {
        private static IServiceProvider _serviceProvider = null;
        /// <summary>
        /// 初始化对象
        /// </summary>
        /// <param name="IServiceProvider"></param>
        public static void RegisterServices(IServiceCollection serviceCollection)
        {
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
        /// <summary>
        /// 获取指定注册服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            if (_serviceProvider != null)
            {
                return _serviceProvider.GetService<T>();
            }
            return default(T);
        }
    }
}
