using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Domain.Shared.Enums
{
    /// <summary>
    /// 通信方式
    /// </summary>
    public enum SignalTypeEnum
    {
        /// <summary>
        /// Http(仅支持post)
        /// </summary>
        [Display(Name = "Http(仅支持POST请求)")]
        Http =0,
        /// <summary>
        /// GRPC
        /// </summary>
        [Display(Name = "GRPC(暂时不支持)")]
        Grpc =1
    }
}
