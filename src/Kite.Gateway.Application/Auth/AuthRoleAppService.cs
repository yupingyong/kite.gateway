using Kite.Gateway.Domain.Entities.Auth;
using Kite.Gateway.Application.Dtos.Auth;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;

namespace Kite.Gateway.Management.Application.Auth
{
    /// <summary>
    /// 角色管理相关接口
    /// </summary>
    public class AuthRoleAppService:BaseApplicationService
    {
        private readonly IRepository<AuthRole> _repository;
        private readonly IRepository<AuthAccount> _accountRepository;
        private readonly IRepository<AuthAuthorize> _authRepository;
        private readonly IRepository<AuthMenu> _menuRepository;

        public AuthRoleAppService(IRepository<AuthRole> repository, IRepository<AuthAccount> accountRepository, IRepository<AuthAuthorize> authRepository, IRepository<AuthMenu> menuRepository)
        {
            _repository = repository;
            _accountRepository = accountRepository;
            _authRepository = authRepository;
            _menuRepository = menuRepository;
        }
        /// <summary>
        /// 设置权限
        /// </summary>
        /// <param name="roleAuthSetDto"></param>
        /// <returns></returns>
        public async Task<KiteResult> PostSetAsync(AuthRoleAuthSetDto roleAuthSetDto)
        {
            await _authRepository.DeleteAsync(x=>x.RoleId== roleAuthSetDto.RoleId);
            var models=new List<AuthAuthorize>();
            if (roleAuthSetDto.MenuIds != null && roleAuthSetDto.MenuIds.Any())
            {
                foreach (var id in roleAuthSetDto.MenuIds)
                {
                    models.Add(new AuthAuthorize(GuidGenerator.Create()) 
                    {
                        MenuId= id,
                        RoleId= roleAuthSetDto.RoleId
                    });
                }
                await _authRepository.InsertManyAsync(models);
            }
            return Ok();
        }
        /// <summary>
        /// 获取权限信息
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        public async Task<KiteResult<List<AuthMenuTreeViewDto>>> GetTreeAsync(Guid id)
        {
            var result = new List<AuthMenuTreeViewDto>();
            //查询菜单信息
            var menus = (await _menuRepository.GetQueryableAsync())
                .OrderBy(x => x.Created)
                .ToList();
            //查询已有权限信息
            var auths = (await _authRepository.GetQueryableAsync())
                .Where(x => x.RoleId == id)
                .ToList();
            //处理权限
            AuthMenuTreeViewDto treeView = null;
            foreach (var area in menus.Where(x => x.ParentId == Guid.Empty).ToList())
            {
                treeView = new AuthMenuTreeViewDto()
                {
                    Checked = false,
                    Disabled = false,
                    Field = "",
                    Id = area.Id,
                    Href = "",
                    Spread = false,
                    Title = area.MenuName,
                    Children = menus.Where(x => x.ParentId == area.Id).Select(x => new AuthMenuTreeViewDto()
                    {
                        Checked = auths.Any(y => y.MenuId == x.Id),
                        Disabled = false,
                        Field = "",
                        Id = x.Id,
                        Href = "",
                        Spread = false,
                        Title = x.MenuName
                    })
                    .ToList()
                };
                result.Add(treeView);

            }
            return Ok(result);
        }
        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public async Task<KitePageResult<List<AuthRoleDto>>> GetPageAsync(int page=1,int limit=10)
        {
            var query = (await _repository.GetQueryableAsync())
                .ProjectToType<AuthRoleDto>();
            var count=query.Count();
            var result = query.OrderByDescending(x => x.Created)
                .PageBy((page - 1) * limit, limit)
                .ToList();
            return Ok(result, count);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<KiteResult<List<AuthRoleDto>>> GetAsync()
        {
            var result = (await _repository.GetQueryableAsync())
                .OrderByDescending(x => x.Created)
                .ProjectToType<AuthRoleDto>()
                .ToList();
            return Ok(result);
        }
        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<KiteResult> DeleteAsync(Guid id)
        {
            if (await _accountRepository.AnyAsync(x => x.RoleId == id))
            {
                return Fail("该角色关联了账号,不允许进行删除");
            }
            await _repository.DeleteAsync(x=>x.Id==id);
            return Ok();
        }
        /// <summary>
        /// 更新角色
        /// </summary>
        /// <param name="adminRoleDto"></param>
        /// <returns></returns>
        public async Task<KiteResult> UpdateAsync(AuthRoleUpdateDto adminRoleDto)
        {
            var model = await _repository.FirstOrDefaultAsync(x=>x.Id==adminRoleDto.Id);
            if (model == null)
            {
                return Fail("角色信息不存在");
            }
            model.RoleName=adminRoleDto.RoleName;
            model.Updated=DateTime.Now;
            await _repository.UpdateAsync(model);
            return Ok();
        }
        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="adminRoleDto"></param>
        /// <returns></returns>
        public async Task<KiteResult> CreateAsync(AuthRoleCreateDto adminRoleDto)
        {
            var model = new AuthRole(GuidGenerator.Create()) 
            {
                Created=DateTime.Now,
                RoleName=adminRoleDto.RoleName,
                Updated=DateTime.Now
            };
            await _repository.InsertAsync(model);
            return Ok();
        }
    }
}
