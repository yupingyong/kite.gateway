using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Kite.Gateway.Application.Contracts.Dtos
{
    public class ValidateTokenDto
    {
        /// <summary>
        /// 请求token
        /// </summary>
        [Required]
        public string AccessToken { get; set; }
        /// <summary>
        /// 请求路径
        /// </summary>
        [Required]
        public string ReuqestPath { get; set; }
    }
}
