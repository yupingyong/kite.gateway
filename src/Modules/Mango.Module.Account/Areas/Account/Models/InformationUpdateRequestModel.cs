using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Account.Areas.Account.Models
{
    public class InformationUpdateRequestModel
    {
        /// <summary>
        /// 账号ID
        /// </summary>
        public int AccountId { get; set; }
        /// <summary>
        /// 待更新的资料信息类型(1.用户昵称 2.用户头像 3.用户标签 4.用户所在地 5.性别)
        /// </summary>
        public int InformationType { get; set; }
        /// <summary>
        /// 待更新的资料值
        /// </summary>
        public string InformationValue { get; set; }
    }
}
