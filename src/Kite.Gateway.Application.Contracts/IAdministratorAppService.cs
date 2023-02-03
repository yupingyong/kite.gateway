﻿using Kite.Gateway.Application.Contracts.Dtos.Administrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kite.Gateway.Application.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAdministratorAppService
    {
        /// <summary>
        /// 管理员登录
        /// </summary>
        /// <param name="loginAdministrator"></param>
        /// <returns></returns>
        Task<KiteResult<AdministratorDto>> LoginAsync(LoginAdministratorDto loginAdministrator);
        /// <summary>
        /// 获取管理员账号详情信息
        /// </summary>
        /// <param name="id">节点ID</param>
        /// <returns></returns>
        Task<KiteResult<AdministratorDto>> GetAsync(int id);
        /// <summary>
        /// 更新管理员账号数据
        /// </summary>
        /// <param name="updateAdministrator"></param>
        /// <returns></returns>
        Task<KiteResult> UpdateAsync(UpdateAdministratorDto updateAdministrator);
        /// <summary>
        /// 获取管理员账号列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<KitePageResult<List<AdministratorDto>>> GetListAsync(int page = 1, int pageSize = 10);
        /// <summary>
        /// 删除管理员账号
        /// </summary>
        /// <param name="id">管理员ID</param>
        /// <returns></returns>
        Task<KiteResult> DeleteAsync(int id);
        /// <summary>
        /// 创建管理员账号
        /// </summary>
        /// <param name="createAdministrator"></param>
        /// <returns></returns>
        Task<KiteResult> CreateAsync(CreateAdministratorDto createAdministrator);
    }
}
