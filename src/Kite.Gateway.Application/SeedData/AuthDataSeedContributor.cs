using Kite.Gateway.Domain.Entities.Auth;
using Kite.Gateway.Domain.Shared.Infrastructure;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;

namespace Kite.Gateway.Domain.SeedData
{
    /// <summary>
    /// 授权种子数据
    /// </summary>
    public class AuthDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<AuthAccount> _accountRepository;
        private readonly IRepository<AuthMenu> _menuRepository;
        private readonly IRepository<AuthRole> _roleRepository;
        private readonly IRepository<AuthAuthorize> _authRepository;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        public AuthDataSeedContributor(IRepository<AuthAccount> accountRepository, IRepository<AuthMenu> menuRepository, IRepository<AuthRole> roleRepository, IRepository<AuthAuthorize> authRepository
            , IGuidGenerator guidGenerator, IUnitOfWorkManager unitOfWorkManager)
        {
            _accountRepository = accountRepository;
            _menuRepository = menuRepository;
            _roleRepository = roleRepository;
            _authRepository = authRepository;
            _guidGenerator = guidGenerator;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            var uow = _unitOfWorkManager.Begin();
            try
            {
                var menus = (await _menuRepository.GetQueryableAsync())
                    .ToList();
                //处理默认菜单
                if (!menus.Any())
                {
                    menus = new List<AuthMenu>();
                    var appsettings = new AuthMenu(_guidGenerator.Create())
                    {
                        Created = DateTime.Now,
                        MenuName = "系统设置",
                        AuthCode = "#",
                        MenuType = Shared.Enums.Auth.MenuTypeEnum.Page,
                        ParentId = Guid.Empty,
                        Updated = DateTime.Now,
                    };
                    menus.Add(appsettings);

                    menus.Add(new AuthMenu(_guidGenerator.Create())
                    {
                        Created = DateTime.Now,
                        MenuName = "权限菜单管理",
                        AuthCode = "/Auth/Menu",
                        MenuType = Shared.Enums.Auth.MenuTypeEnum.Page,
                        ParentId = appsettings.Id,
                        Updated = DateTime.Now,
                    });
                    menus.Add(new AuthMenu(_guidGenerator.Create())
                    {
                        Created = DateTime.Now,
                        MenuName = "管理员账号管理",
                        AuthCode = "/Auth/Account",
                        MenuType = Shared.Enums.Auth.MenuTypeEnum.Page,
                        ParentId = appsettings.Id,
                        Updated = DateTime.Now,
                    });
                    menus.Add(new AuthMenu(_guidGenerator.Create())
                    {
                        Created = DateTime.Now,
                        MenuName = "角色管理",
                        AuthCode = "/Auth/Role",
                        MenuType = Shared.Enums.Auth.MenuTypeEnum.Page,
                        ParentId = appsettings.Id,
                        Updated = DateTime.Now,
                    });
                    await _menuRepository.InsertManyAsync(menus);
                }
                //角色处理
                var role = await _roleRepository.FirstOrDefaultAsync();
                if (role == null)
                {
                    role = new AuthRole(_guidGenerator.Create())
                    {
                        Created = DateTime.Now,
                        RoleName = "超级管理员",
                        Updated = DateTime.Now
                    };
                    await _roleRepository.InsertAsync(role);
                }
                //管理员账号
                var account = await _accountRepository.FirstOrDefaultAsync();
                if (account == null)
                {
                    account = new AuthAccount(_guidGenerator.Create())
                    {
                        Created = DateTime.Now,
                        AccountName = "admin",
                        HeadUrl = "",
                        IsEnable = true,
                        NickName = "admin",
                        Password = TextHelper.MD5Encrypt("123456"),
                        RoleId = role.Id,
                        Updated = DateTime.Now
                    };
                    await _accountRepository.InsertAsync(account);
                }
                //授权信息
                if (!await _authRepository.AnyAsync(x => x.RoleId == role.Id))
                {

                    var authorize = new List<AuthAuthorize>();
                    foreach (var menu in menus)
                    {
                        authorize.Add(new AuthAuthorize(_guidGenerator.Create())
                        {
                            MenuId = menu.Id,
                            RoleId = role.Id
                        });
                    }
                    await _authRepository.InsertManyAsync(authorize);
                }
                await uow.SaveChangesAsync();
                await uow.CompleteAsync();
            }
            catch (Exception ex) 
            {
                await uow.RollbackAsync();
                Log.Error(ex, ex.Message);
            }
        }
    
    }
}
