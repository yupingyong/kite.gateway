using Kite.Gateway.Domain.Entities.Auth;
using Kite.Gateway.Application.Dtos.Auth;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Kite.Gateway.Domain.Shared.Infrastructure;

namespace Kite.Gateway.Application.Auth
{
    /// <summary>
    /// 用户登录相关接口
    /// </summary>
    public class AuthLoginAppService : BaseApplicationService
    {
        private readonly IRepository<AuthAccount> _repository;
        private readonly IRepository<AuthRole> _roleRepository;
        private readonly IRepository<AuthMenu> _menuRepository;
        private readonly IRepository<AuthAuthorize> _authRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthLoginAppService(IRepository<AuthAccount> repository, IRepository<AuthRole> roleRepository, IRepository<AuthMenu> menuRepository, IRepository<AuthAuthorize> authRepository
            , IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _menuRepository = menuRepository;
            _authRepository = authRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        public async Task<KiteResult> PostOutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
        /// <summary>
        /// 管理员账号登录
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public async Task<KiteResult> PostAsync(AccountLoginDto account)
        {
            var password= TextHelper.MD5Encrypt(account.Password);
            var query = from acc in (await _repository.GetQueryableAsync())
                        join role in (await _roleRepository.GetQueryableAsync()) on acc.RoleId equals role.Id
                        where acc.AccountName == account.AccountName && acc.Password == password
                        select new 
                        {
                            acc.RoleId,acc.Id,acc.Created,acc.AccountName,acc.IsEnable,acc.HeadUrl,acc.NickName,role.RoleName
                        };
            var model= query.FirstOrDefault();
            if (model == null)
            {
                throw new Exception("账号或者密码错误");
            }
            if (!model.IsEnable)
            {
                throw new Exception("该账号已经被禁用");
            }
            //写入Cookies
            var claims = new List<Claim>
            {
                new Claim("AccountName", model.AccountName),
                new Claim("NickName", model.NickName),
                new Claim("NickName", model.HeadUrl),
                new Claim("RoleName", model.RoleName),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
            };
            await _httpContextAccessor.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
            return Ok();
        }
    }
}
