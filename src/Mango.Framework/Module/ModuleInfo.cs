using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;

namespace Mango.Framework.Module
{
    public class ModuleInfo
    {
        /// <summary>
        /// 模块ID
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 模块名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get; set; }
        /// <summary>
        /// 模块描述
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// 判断是否属于应用模块(为true则注册到MVC路由中)
        /// </summary>
        public bool IsApplicationPart { get; set; }
        /// <summary>
        /// 模块程序集
        /// </summary>
        public Assembly Assembly { get; set; }
        /// <summary>
        /// 模块视图程序集
        /// </summary>
        public Assembly ViewsAssembly { get; set; }
    }
}
