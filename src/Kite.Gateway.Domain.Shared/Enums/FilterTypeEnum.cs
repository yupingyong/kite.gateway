using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Shared.Enums
{
    /// <summary>
    /// 白名单过滤类型
    /// </summary>
    public enum FilterTypeEnum
    {
        /// <summary>
        /// 路径
        /// </summary>
        [Display(Name = "路径")]
        Path =0,
        /// <summary>
        /// 正则
        /// </summary>
        [Display(Name = "正则")]
        Regular =1
    }
}
