using System;
using System.Collections.Generic;
using System.Text;

namespace Mango.Framework.Services.UPyun
{
    public class UPyunOptions
    {
        /// <summary>
        /// Bucket名称
        /// </summary>
        public string BucketName { get; set; }
        /// <summary>
        /// Bucket空间密码
        /// </summary>
        public string BucketPassword { get; set; }
        /// <summary>
        /// Bucket域名URL地址
        /// </summary>
        public string BucketFileUrl { get; set; }
    }
}
