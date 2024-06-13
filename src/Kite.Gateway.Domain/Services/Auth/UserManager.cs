using Kite.Gateway.Domain.Entities.Auth;
using Kite.Gateway.Domain.Services.Auth.Models;
using Kite.Gateway.Domain.Shared.Enums;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;

namespace Kite.Gateway.Domain.Services.Auth
{
    internal class UserManager : DomainService,IUserManager
    {
        private readonly IRepository<AuthAccount> _repository;
        private readonly IRepository<AuthRole> _roleRepository;
        private readonly IRepository<AuthAuthorize> _authorizeRepository;
        private readonly IRepository<AuthMenu> _menuRepository;
        public UserManager(IRepository<AuthAccount> repository, IRepository<AuthRole> roleRepository, IRepository<AuthAuthorize> authorizeRepository
            , IRepository<AuthMenu> menuRepository)
        {
            _repository = repository;
            _roleRepository = roleRepository;
            _authorizeRepository = authorizeRepository;
            _menuRepository = menuRepository;
        }

        public async Task<LoginResultModel> LoginAsync(string accountName, string password)
        {
            var result=new LoginResultModel();
            if (!await _repository.AnyAsync(x => x.AccountName == accountName && x.Password == password))
            {
                throw new Exception("用户名或密码错误");
            }
            //其它账号登录
            result.User = (await _repository.GetQueryableAsync())
                .Join(await _roleRepository.GetQueryableAsync(), x => x.RoleId, y => y.Id, (x, y) => new UserModel()
                {
                    UserName = accountName,
                    Id = x.Id,
                    RoleId = y.Id,
                    RoleName = y.RoleName,
                    HeadUrl = x.HeadUrl,
                    IsEnable = x.IsEnable,
                    NickName = x.NickName
                })
                .FirstOrDefault();
            var authMenus = (await _authorizeRepository.GetQueryableAsync())
                .Where(x => x.RoleId == result.User.RoleId)
                .Select(x => x.MenuId)
                .ToList();
            result.Menus =(await _menuRepository.GetQueryableAsync()).Where(x => authMenus.Contains(x.Id))
                .Select(x => new MenuModel()
                {
                    Id = x.Id,
                    AuthCode = x.AuthCode,
                    MenuType = x.MenuType,
                    MenuName = x.MenuName,
                    ParentId = x.ParentId,
                    //Children = x.Children.Where(c => authMenus.Contains(c.Id)).ToList()
                })
                .ToList();
            return result;
        }
    }
}
