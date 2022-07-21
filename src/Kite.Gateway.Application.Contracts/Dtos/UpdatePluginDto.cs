using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts.Dtos
{
    public class UpdatePluginDto
    {
        /// <summary>
        /// ID
        /// </summary>
        public Guid Id { get; set; }
        /// <summary>
        /// 中间件名称
        /// </summary>
        [Required]
        public string Name { get; set; }
        /// <summary>
        /// 启用状态(0.关闭 1.启用)
        /// </summary>
        public int UseState { get; set; }
        /// <summary>
        /// 程序集名称
        /// </summary>
        [Required]
        public string AssemblyName { get; set; }
        /// <summary>
        /// 文件路径
        /// </summary>
        [Required]
        public string FilePath { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortCount { get; set; }
        /// <summary>
        /// 中间件描述
        /// </summary>
        [Required]
        public string Description { get; set; }
        /// <summary>
        /// 中间件配置信息
        /// </summary>
        public string Configure { get; set; }
    }
}
