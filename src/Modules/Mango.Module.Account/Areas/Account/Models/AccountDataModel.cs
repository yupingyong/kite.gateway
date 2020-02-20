using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Module.Account.Areas.Account.Models
{
    public class AccountDataModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>

        public string AccountName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>

        public string NickName { get; set; }

        /// <summary>
        /// 用户头像
        /// </summary>

        public string HeadUrl { get; set; }

        /// <summary>
        /// 用户状态(1:正常 0.禁止)
        /// </summary>

        public int StateCode { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>

        public string Phone { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public string Email { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>

        public DateTime RegisterDate { get; set; }

        /// <summary>
        /// 最后登陆时间
        /// </summary>

        public DateTime LastLoginDate { get; set; }

        /// <summary>
        /// 地区信息
        /// </summary>

        public string AddressInfo { get; set; }

        /// <summary>
        /// 生日
        /// </summary>

        public string Birthday { get; set; }

        /// <summary>
        /// 个人标签
        /// </summary>

        public string Tags { get; set; }

        /// <summary>
        /// 性别
        /// </summary>

        public string Sex { get; set; }

        /// <summary>
        /// 
        /// </summary>

        public int GroupId { get; set; }
    }
}
