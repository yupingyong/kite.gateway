using System;
using System.Collections.Generic;
using System.Text;
using Mango.Framework.Module;
namespace Mango.Framework
{
    public static class GlobalConfiguration
    {
        /// <summary>
        /// 模块集合
        /// </summary>
        public static IList<ModuleInfo> Modules { get; set; } = new List<ModuleInfo>();
        public static string ContentRootPath { get; set; }
    }
}
